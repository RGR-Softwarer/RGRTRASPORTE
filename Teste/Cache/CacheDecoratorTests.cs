using Infra.CrossCutting.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text.Json;
using Xunit;

namespace Teste.Cache
{
    public class TestService
    {
        public virtual Task<string> GetDataAsync() => Task.FromResult("test data");
    }

    public class TestCacheDecorator : CacheDecorator<TestService>
    {
        public TestCacheDecorator(TestService service, IDistributedCache cache) 
            : base(service, cache)
        {
        }

        public Task<T> PublicGetOrSetCacheAsync<T>(
            string cacheKey,
            Func<Task<T>> getData,
            TimeSpan? absoluteExpiration = null,
            TimeSpan? slidingExpiration = null)
        {
            return GetOrSetCacheAsync(cacheKey, getData, absoluteExpiration, slidingExpiration);
        }

        public Task PublicInvalidateCacheAsync(string cacheKey)
        {
            return InvalidateCacheAsync(cacheKey);
        }
    }

    public class CacheDecoratorTests
    {
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly Mock<TestService> _serviceMock;
        private readonly TestCacheDecorator _decorator;

        public CacheDecoratorTests()
        {
            _cacheMock = new Mock<IDistributedCache>();
            _serviceMock = new Mock<TestService>();
            _decorator = new TestCacheDecorator(_serviceMock.Object, _cacheMock.Object);
        }

        [Fact]
        public async Task GetOrSetCacheAsync_WhenCacheHit_ReturnsFromCache()
        {
            // Arrange
            var cacheKey = "test_key";
            var cachedData = "cached_value";
            var cachedBytes = JsonSerializer.SerializeToUtf8Bytes(cachedData);

            _cacheMock
                .Setup(x => x.GetAsync(cacheKey, CancellationToken.None))
                .ReturnsAsync(cachedBytes);

            // Act
            var result = await _decorator.PublicGetOrSetCacheAsync(cacheKey, () => Task.FromResult("new_value"));

            // Assert
            Assert.Equal(cachedData, result);
            _serviceMock.Verify(x => x.GetDataAsync(), Times.Never);
        }

        [Fact]
        public async Task GetOrSetCacheAsync_WhenCacheMiss_SetsCache()
        {
            // Arrange
            var cacheKey = "test_key";
            var newData = "new_value";
            DistributedCacheEntryOptions capturedOptions = null;

            _cacheMock
                .Setup(x => x.GetAsync(cacheKey, CancellationToken.None))
                .ReturnsAsync((byte[])null);

            _cacheMock
                .Setup(x => x.SetAsync(cacheKey, It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), CancellationToken.None))
                .Callback<string, byte[], DistributedCacheEntryOptions, CancellationToken>((key, value, options, token) => 
                {
                    capturedOptions = options;
                })
                .Returns(Task.CompletedTask);

            // Act
            var result = await _decorator.PublicGetOrSetCacheAsync(cacheKey, () => Task.FromResult(newData));

            // Assert
            Assert.Equal(newData, result);
            Assert.NotNull(capturedOptions);
            Assert.True(capturedOptions.AbsoluteExpirationRelativeToNow.HasValue);
            Assert.True(capturedOptions.SlidingExpiration.HasValue);

            var deserializedValue = JsonSerializer.Deserialize<string>(_cacheMock.Invocations
                .First(i => i.Method.Name == "SetAsync")
                .Arguments[1] as byte[]);
            Assert.Equal(newData, deserializedValue);
        }

        [Fact]
        public async Task InvalidateCacheAsync_RemovesFromCache()
        {
            // Arrange
            var cacheKey = "test_key";

            _cacheMock
                .Setup(x => x.RemoveAsync(cacheKey, CancellationToken.None))
                .Returns(Task.CompletedTask);

            // Act
            await _decorator.PublicInvalidateCacheAsync(cacheKey);

            // Assert
            _cacheMock.Verify(x => x.RemoveAsync(cacheKey, CancellationToken.None), Times.Once);
        }
    }
} 
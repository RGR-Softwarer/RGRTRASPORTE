using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infra.CrossCutting.Cache
{
    public class CacheDecorator<TService>
    {
        private readonly TService _service;
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _options;

        public CacheDecorator(TService service, IDistributedCache cache)
        {
            _service = service;
            _cache = cache;
            _options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
        }

        protected async Task<T> GetOrSetCacheAsync<T>(
            string cacheKey,
            Func<Task<T>> getData,
            TimeSpan? absoluteExpiration = null,
            TimeSpan? slidingExpiration = null)
        {
            var cachedValue = await _cache.GetAsync(cacheKey);

            if (cachedValue != null)
            {
                return JsonSerializer.Deserialize<T>(cachedValue);
            }

            var result = await getData();

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration ?? _options.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = slidingExpiration ?? _options.SlidingExpiration
            };

            var serializedData = JsonSerializer.SerializeToUtf8Bytes(result);
            await _cache.SetAsync(cacheKey, serializedData, options);

            return result;
        }

        protected async Task InvalidateCacheAsync(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey);
        }
    }
} 
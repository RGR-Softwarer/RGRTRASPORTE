using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infra.CrossCutting.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _options;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
            _options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
        }

        public async Task<T> GetOrCreateAsync<T>(
            string key,
            Func<Task<T>> factory,
            TimeSpan? customExpiration = null)
        {
            var cachedValue = await GetAsync<T>(key);
            if (cachedValue != null)
                return cachedValue;

            var value = await factory();
            await SetAsync(key, value, customExpiration);
            return value;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(value))
                return default;

            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task SetAsync<T>(
            string key,
            T value,
            TimeSpan? customExpiration = null)
        {
            var options = customExpiration.HasValue
                ? new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = customExpiration
                }
                : _options;

            var serializedValue = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, serializedValue, options);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            // Nota: Esta é uma implementação básica.
            // Para uma solução mais robusta, considere usar Redis com SCAN
            throw new NotImplementedException(
                "Implementação de RemoveByPrefix requer uso do Redis com SCAN");
        }
    }
} 
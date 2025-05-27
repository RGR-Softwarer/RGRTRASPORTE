namespace Infra.CrossCutting.Cache
{
    public interface ICacheService
    {
        Task<T> GetOrCreateAsync<T>(
            string key,
            Func<Task<T>> factory,
            TimeSpan? customExpiration = null);

        Task<T> GetAsync<T>(string key);

        Task SetAsync<T>(
            string key,
            T value,
            TimeSpan? customExpiration = null);

        Task RemoveAsync(string key);
        
        Task RemoveByPrefixAsync(string prefix);
    }
} 
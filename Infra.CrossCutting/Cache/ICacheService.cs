namespace Infra.CrossCutting.Cache
{
    public interface ICacheService
    {
        Task<T> GetOrCreateAsync<T>(
            string key,
            Func<CancellationToken, Task<T>> factory,
            TimeSpan? customExpiration = null,
            CancellationToken cancellationToken = default);

        Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);

        Task SetAsync<T>(
            string key,
            T value,
            TimeSpan? customExpiration = null,
            CancellationToken cancellationToken = default);

        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
        
        Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default);
    }
} 

using Infra.CrossCutting.Cache;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Services
{
    /// <summary>
    /// Service for caching frequently accessed query results
    /// </summary>
    public class CachedQueryService
    {
        private readonly ICacheService _cacheService;
        private readonly IMediator _mediator;
        private readonly ILogger<CachedQueryService> _logger;

        // Cache duration configurations
        private readonly TimeSpan _shortCacheDuration = TimeSpan.FromMinutes(5);
        private readonly TimeSpan _mediumCacheDuration = TimeSpan.FromMinutes(30);
        private readonly TimeSpan _longCacheDuration = TimeSpan.FromHours(2);

        public CachedQueryService(
            ICacheService cacheService,
            IMediator mediator,
            ILogger<CachedQueryService> logger)
        {
            _cacheService = cacheService;
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Execute query with caching
        /// </summary>
        public async Task<TResponse> ExecuteWithCacheAsync<TQuery, TResponse>(
            TQuery query,
            CacheDuration duration = CacheDuration.Medium,
            CancellationToken cancellationToken = default)
            where TQuery : IRequest<TResponse>
        {
            var cacheKey = GenerateCacheKey(query);
            var cacheDuration = GetCacheDuration(duration);

            try
            {
                // Try to get from cache first
                var cachedResult = await _cacheService.GetAsync<TResponse>(cacheKey);
                if (cachedResult != null)
                {
                    _logger.LogDebug("Cache hit for key: {CacheKey}", cacheKey);
                    return cachedResult;
                }

                // Execute query
                _logger.LogDebug("Cache miss for key: {CacheKey}. Executing query.", cacheKey);
                var result = await _mediator.Send(query, cancellationToken);

                // Cache the result if not null
                if (result != null)
                {
                    await _cacheService.SetAsync(cacheKey, result, cacheDuration);
                    _logger.LogDebug("Cached result for key: {CacheKey}", cacheKey);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing cached query for key: {CacheKey}", cacheKey);
                
                // Fallback: execute query without caching
                return await _mediator.Send(query, cancellationToken);
            }
        }

        /// <summary>
        /// Invalidate cache for specific patterns
        /// </summary>
        public async Task InvalidateCacheAsync(string pattern)
        {
            try
            {
                await _cacheService.RemoveAsync(pattern);
                _logger.LogInformation("Invalidated cache pattern: {Pattern}", pattern);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error invalidating cache pattern: {Pattern}", pattern);
            }
        }

        /// <summary>
        /// Invalidate entity-specific cache
        /// </summary>
        public async Task InvalidateEntityCacheAsync<T>(long entityId)
        {
            var pattern = $"{typeof(T).Name}:*:{entityId}:*";
            await InvalidateCacheAsync(pattern);
        }

        /// <summary>
        /// Invalidate all cache for an entity type
        /// </summary>
        public async Task InvalidateEntityTypeCacheAsync<T>()
        {
            var pattern = $"{typeof(T).Name}:*";
            await InvalidateCacheAsync(pattern);
        }

        #region Private Methods

        private string GenerateCacheKey<T>(T query)
        {
            try
            {
                var queryType = typeof(T).Name;
                var queryData = JsonSerializer.Serialize(query);
                var queryHash = queryData.GetHashCode().ToString();
                
                return $"{queryType}:{queryHash}";
            }
            catch
            {
                // Fallback to type name if serialization fails
                return $"{typeof(T).Name}:{Guid.NewGuid()}";
            }
        }

        private TimeSpan GetCacheDuration(CacheDuration duration)
        {
            return duration switch
            {
                CacheDuration.Short => _shortCacheDuration,
                CacheDuration.Medium => _mediumCacheDuration,
                CacheDuration.Long => _longCacheDuration,
                _ => _mediumCacheDuration
            };
        }

        #endregion
    }

    /// <summary>
    /// Cache duration options
    /// </summary>
    public enum CacheDuration
    {
        Short,   // 5 minutes - for frequently changing data
        Medium,  // 30 minutes - for moderately changing data
        Long     // 2 hours - for rarely changing data (lookup tables, etc.)
    }
} 
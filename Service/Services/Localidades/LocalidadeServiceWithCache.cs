using Dominio.Dtos.Localidades;
using Dominio.Interfaces.Service.Localidades;
using Infra.CrossCutting.Cache;
using Microsoft.Extensions.Caching.Distributed;

namespace Service.Services.Localidades
{
    public class LocalidadeServiceWithCache : CacheDecorator<ILocalidadeService>, ILocalidadeService
    {
        private readonly ILocalidadeService _localidadeService;
        private const string CACHE_KEY_PREFIX = "localidade:";

        public LocalidadeServiceWithCache(
            ILocalidadeService localidadeService,
            IDistributedCache cache) : base(localidadeService, cache)
        {
            _localidadeService = localidadeService;
        }

        public async Task<LocalidadeDto> ObterPorIdAsync(long id, bool auditado = false)
        {
            return await GetOrSetCacheAsync(
                $"{CACHE_KEY_PREFIX}{id}",
                () => _localidadeService.ObterPorIdAsync(id, auditado));
        }

        public async Task<IEnumerable<LocalidadeDto>> ObterTodosAsync()
        {
            return await GetOrSetCacheAsync(
                $"{CACHE_KEY_PREFIX}all",
                () => _localidadeService.ObterTodosAsync());
        }

        public async Task AdicionarAsync(LocalidadeDto dto)
        {
            await _localidadeService.AdicionarAsync(dto);
            await InvalidateCacheAsync($"{CACHE_KEY_PREFIX}all");
        }

        public async Task EditarAsync(LocalidadeDto dto)
        {
            await _localidadeService.EditarAsync(dto);
            await InvalidateCacheAsync($"{CACHE_KEY_PREFIX}all");
            await InvalidateCacheAsync($"{CACHE_KEY_PREFIX}{dto.Id}");
        }

        public async Task RemoverAsync(long id)
        {
            await _localidadeService.RemoverAsync(id);
            await InvalidateCacheAsync($"{CACHE_KEY_PREFIX}all");
            await InvalidateCacheAsync($"{CACHE_KEY_PREFIX}{id}");
        }

        Task<IEnumerable<LocalidadeDto>> ILocalidadeService.ObterTodosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LocalidadeDto> ObterPorIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
} 
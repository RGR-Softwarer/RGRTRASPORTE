using Application.Queries.Viagem.Models;

namespace Application.Interfaces.Services.Viagens
{
    public interface IGatilhoViagemService
    {
        Task<IEnumerable<GatilhoViagemDto>> ObterTodosAsync();
        Task<GatilhoViagemDto> ObterPorIdAsync(long id);
        Task AdicionarAsync(GatilhoViagemDto dto);
        Task EditarAsync(GatilhoViagemDto dto);
        Task RemoverAsync(long id);
    }
} 
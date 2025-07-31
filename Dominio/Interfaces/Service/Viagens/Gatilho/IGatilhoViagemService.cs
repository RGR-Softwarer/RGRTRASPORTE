using Dominio.Dtos.Viagens.Gatilho;
using Dominio.Entidades.Viagens.Gatilho;

namespace Dominio.Interfaces.Service.Viagens.Gatilho
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

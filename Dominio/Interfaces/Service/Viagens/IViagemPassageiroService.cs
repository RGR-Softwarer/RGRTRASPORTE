using Dominio.Dtos.Viagens;

namespace Dominio.Interfaces.Service.Viagens
{
    public interface IViagemPassageiroService
    {
        Task<IEnumerable<ViagemPassageiroDto>> ObterTodosAsync();
        Task<ViagemPassageiroDto> ObterPorIdAsync(long id);
        Task AdicionarAsync(ViagemPassageiroDto dto);
        Task EditarAsync(ViagemPassageiroDto dto);
        Task RemoverAsync(long id);
    }
}
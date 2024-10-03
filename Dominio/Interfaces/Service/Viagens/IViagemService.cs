using Dominio.Dtos.Viagens;

namespace Dominio.Interfaces.Service.Viagens
{
    public interface IViagemService
    {
        Task<IEnumerable<ViagemDto>> ObterTodosAsync();
        Task<ViagemDto> ObterPorIdAsync(long id);
        Task AdicionarAsync(ViagemDto dto);
        Task EditarAsync(ViagemDto dto);
        Task RemoverAsync(long id);
    }
}
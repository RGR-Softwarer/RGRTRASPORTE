using Dominio.Dtos.Viagens;

namespace Dominio.Interfaces.Service.Viagens
{
    public interface IViagemPosicaoService
    {
        Task<IEnumerable<ViagemPosicaoDto>> ObterTodosAsync();
        Task<ViagemPosicaoDto> ObterPorIdAsync(long id);
        Task AdicionarAsync(ViagemPosicaoDto dto);
        Task EditarAsync(ViagemPosicaoDto dto);
        Task RemoverAsync(long id);
    }
}
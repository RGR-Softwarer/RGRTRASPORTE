using Dominio.Dtos;

namespace Dominio.Interfaces.Service
{
    public interface IVeiculoService
    {
        Task<List<VeiculoDto>> ObterTodosAsync();
        Task<VeiculoDto> ObterPorIdAsync(int id);
        Task AdicionarAsync(VeiculoDto dto);
        Task AdicionarEmLoteAsync(List<VeiculoDto> dto);
        void EditarAsync(VeiculoDto dto);
        void EditarEmLoteAsync(List<VeiculoDto> dto);
        Task RemoverAsync(int id);
    }
}

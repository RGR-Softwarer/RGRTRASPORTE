using Dominio.Dtos.Veiculo;

namespace Dominio.Interfaces.Service
{
    public interface IModeloVeicularService
    {
        Task<List<ModeloVeicularDto>> ObterTodosAsync();
        Task<ModeloVeicularDto> ObterPorIdAsync(long id);
        Task AdicionarAsync(ModeloVeicularDto dto);
        Task EditarAsync(ModeloVeicularDto dto);
        Task RemoverAsync(long id);
    }
}

using Application.Queries.Veiculo.ModeloVeicular.Models;

namespace Application.Interfaces.Services
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
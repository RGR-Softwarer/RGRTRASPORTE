using Dominio.Dtos;
using Dominio.Entidades.Veiculos;

namespace Dominio.Interfaces.Infra.Data
{
    public interface IVeiculoRepository
    {
        Task<List<Veiculo>> ObterTodosAsync();
        Task<Veiculo> ObterPorIdAsync(int id);
        Task AdicionarAsync(Veiculo veiculo);
        Task AdicionarEmLoteAsync(List<Veiculo> veiculo);
        void Editar(Veiculo veiculo);
        void EditarEmLoteAsync(List<Veiculo> dto);
        void Remover(Veiculo veiculo);
    }
}

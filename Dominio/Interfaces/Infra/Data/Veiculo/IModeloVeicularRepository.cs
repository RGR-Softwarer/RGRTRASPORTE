using Dominio.Dtos.Auditoria;
using Dominio.Entidades.Veiculos;

namespace Dominio.Interfaces.Infra.Data.Veiculo
{
    public interface IModeloVeicularRepository
    {
        IQueryable<ModeloVeicular> Query();
        Task<List<ModeloVeicular>> ObterTodosAsync();
        Task<ModeloVeicular> ObterPorIdAsync(long id, bool auditado = false);
        Task AdicionarEmLoteAsync(List<ModeloVeicular> listaEntidades);
        Task RemoverAsync(ModeloVeicular entidade);
        Task RemoverEmLoteAsync(List<ModeloVeicular> listaEntidades);
        Task AtualizarAsync(ModeloVeicular entidade, AuditadoDto auditado = null);
        Task AtualizarEmLoteAsync(List<ModeloVeicular> listaEntidades);
        Task AdicionarAsync(ModeloVeicular entidade, AuditadoDto auditado = null);
    }
}

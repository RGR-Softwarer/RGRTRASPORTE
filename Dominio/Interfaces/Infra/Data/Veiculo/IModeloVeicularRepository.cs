using Dominio.Dtos.Auditoria;
using Dominio.Entidades.Veiculo;

namespace Dominio.Interfaces.Infra.Data
{
    public interface IModeloVeicularRepository
    {
        IQueryable<ModeloVeicular> Query();
        Task<List<ModeloVeicular>> ObterTodosAsync();
        Task<ModeloVeicular> ObterPorIdAsync(long id, bool auditado = false);
        Task AdicionarEmLoteAsync(List<ModeloVeicular> listaEntidades);
        void Remover(ModeloVeicular entidade);
        void RemoverEmLoteAsync(List<ModeloVeicular> listaEntidades);
        void Atualizar(ModeloVeicular entidade, AuditadoDto auditado = null);
        void AtualizarEmLoteAsync(List<ModeloVeicular> listaEntidades);
        Task AdicionarAsync(ModeloVeicular entidade, AuditadoDto auditado = null);
    }
}

using Dominio.Dtos.Auditoria;

namespace Dominio.Interfaces.Infra.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Query();
        Task<List<T>> ObterTodosAsync();
        Task<T> ObterPorIdAsync(long id, bool auditado = false);
        Task AdicionarEmLoteAsync(List<T> listaEntidades);
        void Remover(T entidade);
        void RemoverEmLoteAsync(List<T> listaEntidades);
        void Atualizar(T entidade, AuditadoDto auditado = null);
        void AtualizarEmLoteAsync(List<T> listaEntidades);
        Task AdicionarAsync(T entidade, AuditadoDto auditado = null);
    }
}

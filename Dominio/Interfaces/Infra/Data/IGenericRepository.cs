using Dominio.Dtos.Auditoria;

namespace Dominio.Interfaces.Infra.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Query();
        Task<List<T>> ObterTodosAsync();
        Task<T> ObterPorIdAsync(long id, bool auditado = false);
        Task AdicionarEmLoteAsync(List<T> listaEntidades);
        Task RemoverAsync(T entidade);
        Task RemoverEmLoteAsync(List<T> listaEntidades);
        Task AtualizarAsync(T entidade, AuditadoDto auditado = null);
        Task AtualizarEmLoteAsync(List<T> listaEntidades);
        Task AdicionarAsync(T entidade, AuditadoDto auditado = null);
    }
}

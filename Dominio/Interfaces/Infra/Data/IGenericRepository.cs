using Dominio.Dtos.Auditoria;
using System.Linq.Expressions;

namespace Dominio.Interfaces.Infra.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Query();
        Task<(IEnumerable<T> Items, int Total)> GetPaginatedAsync(
                int pageNumber,
                int pageSize,
                string orderByProperty = "",
                bool isDescending = false,
                Expression<Func<T, bool>> filter = null); 
        Task<List<T>> ObterTodosAsync(CancellationToken cancellationToken = default);
        Task<T> ObterPorIdAsync(long id, bool auditado = false, CancellationToken cancellationToken = default);
        Task AdicionarEmLoteAsync(List<T> listaEntidades, CancellationToken cancellationToken = default);
        Task RemoverAsync(T entidade);
        Task RemoverEmLoteAsync(List<T> listaEntidades);
        Task AtualizarAsync(T entidade, AuditadoDto auditado = null);
        Task AtualizarEmLoteAsync(List<T> listaEntidades);
        Task AdicionarAsync(T entidade, AuditadoDto auditado = null, CancellationToken cancellationToken = default);
    }
}

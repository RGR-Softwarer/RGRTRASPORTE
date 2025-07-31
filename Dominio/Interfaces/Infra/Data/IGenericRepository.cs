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
                Expression<Func<T, bool>> filter = null,
                CancellationToken cancellationToken = default); 
        Task<List<T>> ObterTodosAsync(CancellationToken cancellationToken = default);
        Task<T> ObterPorIdAsync(long id, CancellationToken cancellationToken = default);
        Task AdicionarEmLoteAsync(List<T> listaEntidades, CancellationToken cancellationToken = default);
        Task RemoverAsync(T entidade, CancellationToken cancellationToken = default);
        Task RemoverEmLoteAsync(List<T> listaEntidades, CancellationToken cancellationToken = default);
        Task AtualizarAsync(T entidade, CancellationToken cancellationToken = default);
        Task AtualizarEmLoteAsync(List<T> listaEntidades, CancellationToken cancellationToken = default);
        Task AdicionarAsync(T entidade, CancellationToken cancellationToken = default);
    }
}

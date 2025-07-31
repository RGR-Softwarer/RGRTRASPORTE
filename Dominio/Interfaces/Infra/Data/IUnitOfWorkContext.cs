using Microsoft.EntityFrameworkCore;

namespace Dominio.Interfaces.Infra.Data
{
    public interface IUnitOfWorkContext
    {
        DbSet<T> Set<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

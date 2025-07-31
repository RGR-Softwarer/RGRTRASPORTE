using Dominio.Interfaces.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWorkContext _dbContext;

        public UnitOfWork(IUnitOfWorkContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Commit(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RollBack(CancellationToken cancellationToken = default)
        {
            // Recarrega todas as entidades rastreadas
            if (_dbContext is DbContext context)
            {
                var tasks = context.ChangeTracker.Entries()
                        .Select(entry => entry.ReloadAsync(cancellationToken)).ToList();

                await Task.WhenAll(tasks);
            }
        }
    }
}

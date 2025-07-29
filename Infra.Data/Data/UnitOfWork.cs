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

        public async Task<int> Commit()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task RollBack()
        {
            // Recarrega todas as entidades rastreadas
            if (_dbContext is DbContext context)
            {
                var tasks = context.ChangeTracker.Entries()
                        .Select(entry => entry.ReloadAsync()).ToList();

                await Task.WhenAll(tasks);
            }
        }
    }
}

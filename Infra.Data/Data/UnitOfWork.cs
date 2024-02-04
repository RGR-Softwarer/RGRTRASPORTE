using Dominio.Interfaces.Infra.Data;
using Infra.Data.Context;
using System.Threading.Tasks;

namespace Infra.Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RGRContext _dbContext;

        public UnitOfWork(RGRContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> Commit()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task RollBack()
        {
            var tasks = _dbContext.ChangeTracker.Entries()
                    .Select(entry => entry.ReloadAsync()).ToList();

            await Task.WhenAll(tasks);
        }
    }
}

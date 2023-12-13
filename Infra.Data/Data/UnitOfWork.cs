using Dominio.Interfaces.Infra.Data;
using Infra.Data.Context;

namespace Infra.Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RGRContext _dbContext;


        public UnitOfWork(RGRContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public void RollBack()
        {
            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }
    }
}

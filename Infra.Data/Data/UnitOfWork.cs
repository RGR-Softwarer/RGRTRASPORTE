using Dominio.Entidades;
using Dominio.Entidades.Auditoria;
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

        public async Task<int> Commit()
        {
            int result = await _dbContext.SaveChangesAsync();

            foreach (var (entity, historicoObjeto) in _dbContext.PendingEntities)
            {
                await ProcessarAuditoriaAsync(entity, historicoObjeto);
            }

            _dbContext.PendingEntities.Clear();

            return result;
        }

        public async Task RollBack()
        {
            var tasks = _dbContext.ChangeTracker.Entries()
                    .Select(entry => entry.ReloadAsync()).ToList();

            await Task.WhenAll(tasks);
        }

        private Task ProcessarAuditoriaAsync(BaseEntity entity, HistoricoObjeto historicoObjeto)
        {
            historicoObjeto.CodigoObjeto = entity.Id;

            _dbContext.Set<HistoricoObjeto>().Add(historicoObjeto);
            return _dbContext.SaveChangesAsync();
        }
    }
}

using Dominio.Entidades;
using Dominio.Entidades.Auditoria;
using Dominio.Interfaces.Infra.Data;

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
            var tasks = _dbContext.GetChangeTracker().Entries()
                    .Select(entry => entry.ReloadAsync()).ToList();

            await Task.WhenAll(tasks);
        }

        private async Task ProcessarAuditoriaAsync(BaseEntity entity, HistoricoObjeto historicoObjeto)
        {
            historicoObjeto.CodigoObjeto = entity.Id;
            historicoObjeto.DescricaoObjeto = entity.DescricaoAuditoria;

            _dbContext.Set<HistoricoObjeto>().Add(historicoObjeto);

            await _dbContext.SaveChangesAsync();
        }
    }
}

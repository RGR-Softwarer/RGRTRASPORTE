using Dominio.Entidades;
using Dominio.Entidades.Auditoria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Dominio.Interfaces.Infra.Data
{
    public interface IUnitOfWorkContext : IDisposable
    {
        DatabaseFacade Database { get; }
        ChangeTracker ChangeTracker { get; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges(bool acceptAllChangesOnSuccess);

        /// <summary>
        /// Se implementado, permite armazenar entidades auditáveis.
        /// </summary>
        List<(BaseEntity Entity, HistoricoObjeto HistoricoObjeto)> PendingEntities { get; }

        ChangeTracker GetChangeTracker();
        EntityEntry GetEntry(object entity);
    }
}

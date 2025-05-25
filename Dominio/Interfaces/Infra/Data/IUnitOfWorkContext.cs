using Dominio.Entidades;
using Dominio.Entidades.Auditoria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Dominio.Interfaces.Infra.Data
{
    public interface IUnitOfWorkContext
    {
        DbSet<T> Set<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges(bool acceptAllChangesOnSuccess);

        /// <summary>
        /// Se implementado, permite armazenar entidades auditáveis.
        /// </summary>
        List<(BaseEntity Entity, HistoricoObjeto HistoricoObjeto)>? PendingEntities { get; }

        ChangeTracker GetChangeTracker();
        EntityEntry GetEntry(object entity);
    }
}

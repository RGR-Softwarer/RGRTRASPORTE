using Dominio.Entidades;
using Dominio.Entidades.Auditoria;
using Dominio.Entidades.Veiculos;
using Infra.Data.Configurators.Veiculo;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Context
{
    public class RGRContext : DbContext
    {
        public List<(BaseEntity Entity, HistoricoObjeto HistoricoObjeto)> PendingEntities { get; } = new();

        public RGRContext(DbContextOptions<RGRContext> options) : base(options) { }

        #region DbSets

        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<ModeloVeicular> ModeloVeicular { get; set; }
        public DbSet<HistoricoObjeto> HistoricoObjeto { get; set; }
        public DbSet<HistoricoPropriedade> HistoricoPropriedade { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Method intentionally left empty.
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VeiculoConfigurator).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ModeloVeicularConfigurator).Assembly);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedAt = now;
                            break;

                        case EntityState.Added:
                            trackable.CreatedAt = now;
                            trackable.UpdatedAt = now;
                            break;
                    }
                }
            }
        }
    }
}

using Dominio.Entidades;
using Dominio.Entidades.Auditoria;
using Dominio.Entidades.Veiculos;
using Dominio.Entidades.Viagens;
using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Interfaces.Infra.Data;
using Infra.Data.Configurators.Auditoria;
using Infra.Data.Configurators.Veiculo;
using Infra.Data.Configurators.Viagens;
using Infra.Data.Configurators.Viagens.Gatilho;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infra.Data.Context
{
    public class TransportadorContext : DbContext, IUnitOfWorkContext
    {
        public TransportadorContext(DbContextOptions<TransportadorContext> options) : base(options) { }

        public List<(BaseEntity Entity, HistoricoObjeto HistoricoObjeto)> PendingEntities { get; } = new();

        #region DbSets

        public DbSet<Viagem> Viagens { get; set; }
        public DbSet<ViagemPassageiro> ViagemPassageiros { get; set; }
        public DbSet<ViagemPosicao> ViagemPosicoes { get; set; }

        public DbSet<GatilhoViagem> GatilhosViagem { get; set; }

        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<ModeloVeicular> ModelosVeiculares { get; set; }

        public DbSet<HistoricoObjeto> HistoricoObjeto { get; set; }
        public DbSet<HistoricoPropriedade> HistoricoPropriedade { get; set; }


        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ViagemConfigurator());
            modelBuilder.ApplyConfiguration(new ViagemPassageiroConfigurator());
            modelBuilder.ApplyConfiguration(new ViagemPosicaoConfigurator());

            modelBuilder.ApplyConfiguration(new GatilhoViagemConfigurator());

            modelBuilder.ApplyConfiguration(new VeiculoConfigurator());
            modelBuilder.ApplyConfiguration(new ModeloVeicularConfigurator());

            modelBuilder.ApplyConfiguration(new HistoricoObjetoConfigurator());
            modelBuilder.ApplyConfiguration(new HistoricoPropriedadeConfigurator());
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var now = DateTime.UtcNow;
            foreach (var entry in GetChangeTracker().Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
            }
        }

        public ChangeTracker GetChangeTracker()
        {
            return base.ChangeTracker;
        }

        public EntityEntry GetEntry(object entity)
        {
            return base.Entry(entity);
        }
    }
}
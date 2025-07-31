using Dominio.Entidades;
using Dominio.Entidades.Auditoria;
using Dominio.Entidades.Pessoas;
using Dominio.Entidades.Veiculos;
using Dominio.Entidades.Viagens;
using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Interfaces.Infra.Data;
using Infra.Data.Configurators.Auditoria;
using Infra.Data.Configurators.Pessoa;
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

        // Entidades de Viagens
        public DbSet<Viagem> Viagens { get; set; }
        public DbSet<ViagemPassageiro> ViagemPassageiros { get; set; }
        public DbSet<ViagemPosicao> ViagemPosicoes { get; set; }
        public DbSet<GatilhoViagem> GatilhoViagens { get; set; }

        // Entidades de Veículos
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<ModeloVeicular> ModelosVeiculares { get; set; }

        // Auditoria - Disponível em ambos os contextos
        public DbSet<RegistroAuditoria> RegistrosAuditoria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ignorar Value Objects que não devem ser mapeados como entidades
            modelBuilder.Ignore<Dominio.ValueObjects.Endereco>();
            modelBuilder.Ignore<Dominio.ValueObjects.CPF>();
            modelBuilder.Ignore<Dominio.ValueObjects.Placa>();
            modelBuilder.Ignore<Dominio.ValueObjects.CodigoViagem>();
            modelBuilder.Ignore<Dominio.ValueObjects.Polilinha>();
            modelBuilder.Ignore<Dominio.ValueObjects.PeriodoViagem>();
            modelBuilder.Ignore<Dominio.ValueObjects.Distancia>();
            modelBuilder.Ignore<Dominio.Events.Base.DomainEvent>();

            // Configurações das entidades
            modelBuilder.ApplyConfiguration(new ViagemConfigurator());
            modelBuilder.ApplyConfiguration(new ViagemPassageiroConfigurator());
            modelBuilder.ApplyConfiguration(new ViagemPosicaoConfigurator());
            modelBuilder.ApplyConfiguration(new GatilhoViagemConfigurator());
            modelBuilder.ApplyConfiguration(new VeiculoConfigurator());
            modelBuilder.ApplyConfiguration(new ModeloVeicularConfigurator());
            modelBuilder.ApplyConfiguration(new RegistroAuditoriaConfigurator());
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

        public DbSet<T> Set<T>() where T : class => base.Set<T>();
    }
}

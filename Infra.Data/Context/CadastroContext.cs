using Dominio.Entidades;
using Dominio.Entidades.Auditoria;
using Dominio.Entidades.Localidades;
using Dominio.Entidades.Pessoas;
using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Interfaces.Infra.Data;
using Infra.Data.Configurators.Localidades;
using Infra.Data.Configurators.Pessoa;
using Infra.Data.Configurators.Pessoa.Passageiros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infra.Data.Context
{
    public class CadastroContext : DbContext, IUnitOfWorkContext
    {
        public CadastroContext(DbContextOptions<CadastroContext> options) : base(options) { }
        public List<(BaseEntity Entity, HistoricoObjeto HistoricoObjeto)>? PendingEntities => null;

        public DbSet<Motorista> Motoristas { get; set; }
        public DbSet<Passageiro> Passageiros { get; set; }
        public DbSet<Localidade> Localidades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MotoristaConfigurator());
            modelBuilder.ApplyConfiguration(new PassageiroConfigurator());
            modelBuilder.ApplyConfiguration(new LocalidadeConfigurator());
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
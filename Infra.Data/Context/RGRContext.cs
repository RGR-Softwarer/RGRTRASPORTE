using Dominio.Entidades;
using Dominio.Entidades.Auditoria;
using Dominio.Entidades.Veiculos;
using Infra.Data.Configurators;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Context
{
    public class RGRContext : DbContext
    {
        public List<(BaseEntity Entity, HistoricoObjeto HistoricoObjeto)> PendingEntities = new();

        public RGRContext(DbContextOptions<RGRContext> options) : base(options) { }

        #region DbSets

        public DbSet<Veiculo> Veiculos { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Method intentionally left empty.
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VeiculoConfigurator).Assembly);
        }
    }
}

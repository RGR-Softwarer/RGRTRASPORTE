using Dominio.Entidades;
using Infra.Data.Configurators;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Context
{
    public class RGRContext : DbContext
    {

        public RGRContext(DbContextOptions<RGRContext> options) : base(options) { }

        #region DbSets

        public DbSet<Veiculo> Veiculos { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VeiculoConfigurator).Assembly);
        }
    }
}

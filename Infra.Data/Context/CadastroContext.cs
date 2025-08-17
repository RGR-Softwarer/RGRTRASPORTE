using Dominio.Entidades;
using Dominio.Entidades.Auditoria;
using Dominio.Entidades.Localidades;
using Dominio.Entidades.Pessoas;
using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Entidades.Veiculos;
using Dominio.Interfaces.Infra.Data;
using Infra.Data.Configurators.Auditoria;
using Infra.Data.Configurators.Localidades;
using Infra.Data.Configurators.Pessoa;
using Infra.Data.Configurators.Pessoa.Passageiros;
using Infra.Data.Configurators.Veiculo;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Context
{
    public class CadastroContext : DbContext, IUnitOfWorkContext
    {
        public CadastroContext(DbContextOptions<CadastroContext> options) : base(options) { }

        // Entidades de Cadastro
        public DbSet<Localidade> Localidades { get; set; }
        public DbSet<Passageiro> Passageiros { get; set; }
        public DbSet<Motorista> Motoristas { get; set; }

        // Auditoria - Nova implementa��o
        public DbSet<RegistroAuditoria> RegistrosAuditoria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ignorar Value Objects que n�o devem ser mapeados como entidades
            modelBuilder.Ignore<Dominio.ValueObjects.Endereco>();
            modelBuilder.Ignore<Dominio.ValueObjects.CPF>();
            modelBuilder.Ignore<Dominio.ValueObjects.Placa>();
            modelBuilder.Ignore<Dominio.ValueObjects.CNH>();
            modelBuilder.Ignore<Dominio.ValueObjects.Chassi>();
            modelBuilder.Ignore<Dominio.ValueObjects.CapacidadeVeiculo>();
            modelBuilder.Ignore<Dominio.ValueObjects.Dinheiro>();
            modelBuilder.Ignore<Dominio.Events.Base.DomainEvent>();

            // Configura��es das entidades
            modelBuilder.ApplyConfiguration(new LocalidadeConfigurator());
            modelBuilder.ApplyConfiguration(new PassageiroConfigurator());
            modelBuilder.ApplyConfiguration(new MotoristaConfigurator());
            modelBuilder.ApplyConfiguration(new RegistroAuditoriaConfigurator());
        }

        public DbSet<T> Set<T>() where T : class => base.Set<T>();
    }
}

using Dominio.Entidades.Viagens.Gatilho;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Viagens.Gatilho
{
    internal class GatinhoViagemConfigurator : BaseEntityConfigurator<GatilhoViagem>
    {
        protected override void InternalConfigure(EntityTypeBuilder<GatilhoViagem> builder)
        {
            builder.Property(g => g.Descricao).IsRequired();
            builder.Property(g => g.VeiculoId).IsRequired();
            builder.Property(g => g.MotoristaId).IsRequired();
            builder.Property(g => g.OrigemId).IsRequired();
            builder.Property(g => g.DestinoId).IsRequired();
            builder.Property(g => g.HorarioSaida).IsRequired();
            builder.Property(g => g.HorarioChegada).IsRequired();
            builder.Property(g => g.Ativo).IsRequired();
            builder.Property(g => g.Distancia).IsRequired();

            builder.HasOne(g => g.Veiculo).WithMany().HasForeignKey(g => g.VeiculoId);
            builder.HasOne(g => g.Motorista).WithMany().HasForeignKey(g => g.MotoristaId);
            builder.HasOne(g => g.Origem).WithMany().HasForeignKey(g => g.OrigemId);
            builder.HasOne(g => g.Destino).WithMany().HasForeignKey(g => g.DestinoId);
            builder.HasMany(g => g.Viagem).WithOne(g => g.GatinhoViagem).HasForeignKey(g => g.GatinhoViagemId);

            builder.ToTable(nameof(GatilhoViagem));
        }
    }
}
using Dominio.Entidades.Viagens;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Viagens
{
    internal class ViagemConfigurator : BaseEntityConfigurator<Viagem>
    {
        protected override void InternalConfigure(EntityTypeBuilder<Viagem> builder)
        {
            builder.Property(v => v.CodigoViagem).IsRequired();
            builder.Property(v => v.DataViagem).IsRequired();
            builder.Property(v => v.VeiculoId).IsRequired();
            builder.Property(v => v.MotoristaId).IsRequired();
            builder.Property(v => v.OrigemId).IsRequired();
            builder.Property(v => v.DestinoId).IsRequired();
            builder.Property(v => v.HorarioSaida).IsRequired();
            builder.Property(v => v.HorarioChegada).IsRequired();
            builder.Property(v => v.Situacao).IsRequired();
            builder.Property(v => v.Distancia).IsRequired();
            builder.Property(v => v.NumeroPassageiros).IsRequired();
            builder.Property(v => v.Lotado).IsRequired();
            builder.Property(v => v.Excesso).IsRequired();

            builder.HasOne(v => v.Veiculo).WithMany().HasForeignKey(v => v.VeiculoId);
            builder.HasOne(v => v.Motorista).WithMany().HasForeignKey(v => v.MotoristaId);
            builder.HasOne(v => v.Origem).WithMany().HasForeignKey(v => v.OrigemId);
            builder.HasOne(v => v.Destino).WithMany().HasForeignKey(v => v.DestinoId);
            builder.HasOne(v => v.GatinhoViagem).WithMany().HasForeignKey(v => v.GatinhoViagemId);

            builder.ToTable(nameof(Viagem));
        }
    }
}
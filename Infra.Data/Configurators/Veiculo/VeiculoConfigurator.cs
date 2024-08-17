using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Veiculo
{
    internal class VeiculoConfigurator : BaseEntityConfigurator<Dominio.Entidades.Veiculo.Veiculo>
    {
        protected override void InternalConfigure(EntityTypeBuilder<Dominio.Entidades.Veiculo.Veiculo> builder)
        {
            builder.Property(p => p.Placa).IsRequired();
            builder.Property(p => p.Modelo).IsRequired();
            builder.Property(p => p.Marca).IsRequired();
            builder.Property(p => p.AnoFabricacao).IsRequired();
            builder.Property(p => p.NumeroChassi).IsRequired();
            builder.Property(p => p.AnoModelo).IsRequired();
            builder.Property(p => p.Cor);
            builder.Property(p => p.Renavam).IsRequired();
            builder.Property(p => p.TipoCombustivel).IsRequired();
            builder.Property(p => p.Status).IsRequired();
            builder.Property(p => p.Observacao);
            builder.Property(p => p.VencimentoLicenciamento);
            builder.Property(p => p.ModeloVeiculoId);
            builder.HasOne(h => h.ModeloVeiculo).WithMany(p => p.Veiculos).HasForeignKey(h => h.ModeloVeiculoId);

            builder.ToTable(nameof(Veiculo));
        }
    }
}
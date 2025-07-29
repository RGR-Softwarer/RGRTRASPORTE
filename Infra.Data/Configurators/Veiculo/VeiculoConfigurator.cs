using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Veiculo
{
    internal class VeiculoConfigurator : BaseEntityConfigurator<Dominio.Entidades.Veiculos.Veiculo>
    {
        protected override void InternalConfigure(EntityTypeBuilder<Dominio.Entidades.Veiculos.Veiculo> builder)
        {
            prefixo = "VEI";

            builder.ToTable("T_VEICULO");

            builder.Property(p => p.Placa)
                .HasConversion(
                    placa => placa.Numero,
                    numero => new Dominio.ValueObjects.Placa(numero))
                .IsRequired()
                .HasColumnName($"{prefixo}_PLACA");

            builder.Property(p => p.Modelo)
                .IsRequired()
                .HasColumnName($"{prefixo}_MODELO");

            builder.Property(p => p.Marca)
                .IsRequired()
                .HasColumnName($"{prefixo}_MARCA");

            builder.Property(p => p.AnoFabricacao)
                .IsRequired()
                .HasColumnName($"{prefixo}_ANO_FABRICACAO");

            builder.Property(p => p.NumeroChassi)
                .IsRequired()
                .HasColumnName($"{prefixo}_NUMERO_CHASSI");

            builder.Property(p => p.AnoModelo)
                .IsRequired()
                .HasColumnName($"{prefixo}_ANO_MODELO");

            builder.Property(p => p.Cor)
                .HasColumnName($"{prefixo}_COR");

            builder.Property(p => p.Renavam)
                .IsRequired()
                .HasColumnName($"{prefixo}_RENAVAM");

            builder.Property(p => p.TipoCombustivel)
                .IsRequired()
                .HasColumnName($"{prefixo}_TIPO_COMBUSTIVEL");

            builder.Property(p => p.Status)
                .IsRequired()
                .HasColumnName($"{prefixo}_STATUS");

            builder.Property(p => p.Observacao)
                .HasColumnName($"{prefixo}_OBSERVACAO");

            builder.Property(p => p.VencimentoLicenciamento)
                .HasColumnName($"{prefixo}_VENCIMENTO_LICENCIAMENTO");

            builder.Property(p => p.ModeloVeiculoId)
                .HasColumnName($"MOV_ID");

            builder.HasOne(h => h.ModeloVeiculo)
                .WithMany(p => p.Veiculos)
                .HasForeignKey(h => h.ModeloVeiculoId);

            builder.Property(p => p.Situacao)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName($"{prefixo}_SITUACAO");
        }
    }
}

using Dominio.Entidades.Veiculos;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Veiculo
{
    internal class ModeloVeicularConfigurator : BaseEntityConfigurator<ModeloVeicular>
    {
        protected override void InternalConfigure(EntityTypeBuilder<ModeloVeicular> builder)
        {
            prefixo = "MOV";

            builder.ToTable("T_MODELO_VEICULAR");

            builder.Property(p => p.Situacao)
                .IsRequired()
                .HasColumnName($"{prefixo}_SITUACAO");

            builder.Property(p => p.DescricaoModelo)
                .IsRequired()
                .HasColumnName($"{prefixo}_DESCRICAO_MODELO");

            builder.Property(p => p.Tipo)
                .IsRequired()
                .HasColumnName($"{prefixo}_TIPO");

            builder.Property(p => p.QuantidadeAssento)
                .IsRequired()
                .HasColumnName($"{prefixo}_QUANTIDADE_ASSENTO");

            builder.Property(p => p.QuantidadeEixo)
                .IsRequired()
                .HasColumnName($"{prefixo}_QUANTIDADE_EIXO");

            builder.Property(p => p.CapacidadeMaxima)
                .IsRequired()
                .HasColumnName($"{prefixo}_CAPACIDADE_MAXIMA");

            builder.Property(p => p.PassageirosEmPe)
                .IsRequired()
                .HasColumnName($"{prefixo}_PASSAGEIROS_EM_PE");

            builder.Property(p => p.PossuiBanheiro)
                .IsRequired()
                .HasColumnName($"{prefixo}_POSSUI_BANHEIRO");

            builder.Property(p => p.PossuiClimatizador)
                .IsRequired()
                .HasColumnName($"{prefixo}_POSSUI_CLIMATIZADOR");

            builder.HasMany(m => m.Veiculos)
                .WithOne(v => v.ModeloVeiculo)
                .HasForeignKey(v => v.ModeloVeiculoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
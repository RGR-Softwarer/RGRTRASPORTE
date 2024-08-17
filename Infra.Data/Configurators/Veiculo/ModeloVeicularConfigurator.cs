using Dominio.Entidades.Veiculo;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Veiculo
{
    internal class ModeloVeicularConfigurator : BaseEntityConfigurator<ModeloVeicular>
    {
        protected override void InternalConfigure(EntityTypeBuilder<ModeloVeicular> builder)
        {
            builder.Property(p => p.Situacao).IsRequired();
            builder.Property(p => p.DescricaoModelo).IsRequired();
            builder.Property(p => p.Tipo).IsRequired();
            builder.Property(p => p.QuantidadeAssento).IsRequired();
            builder.Property(p => p.QuantidadeEixo).IsRequired();
            builder.Property(p => p.CapacidadeMaxima).IsRequired();
            builder.Property(p => p.PassageirosEmPe).IsRequired();
            builder.Property(p => p.PossuiBanheiro).IsRequired();
            builder.Property(p => p.PossuiClimatizador).IsRequired();

            builder.HasMany(m => m.Veiculos).WithOne(v => v.ModeloVeiculo).HasForeignKey(v => v.ModeloVeiculoId).OnDelete(DeleteBehavior.Restrict);

            builder.ToTable(nameof(ModeloVeicular));
        }
    }
}
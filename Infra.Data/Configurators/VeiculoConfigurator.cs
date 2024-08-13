using Dominio.Entidades.Veiculos;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators
{
    internal class VeiculoConfigurator : BaseEntityConfigurator<Veiculo>
    {
        protected override void InternalConfigure(EntityTypeBuilder<Veiculo> builder)
        {
            builder.Property(p => p.Placa).IsRequired();
            builder.Property(p => p.Modelo);
            builder.Property(p => p.Marca);
            builder.Property(p => p.Ano);
            builder.Property(p => p.Cor);
            builder.Property(p => p.Renavam).IsRequired();
            builder.Property(p => p.TipoCombustivel).IsRequired();
            builder.Property(p => p.TipoVeiculo).IsRequired();
            builder.Property(p => p.Capacidade).IsRequired();
            builder.Property(p => p.CategoriaCNH).IsRequired();
            builder.Property(p => p.Status).IsRequired();
            builder.Property(p => p.Observacao);
            builder.ToTable(nameof(Veiculo));
        }
    }
}
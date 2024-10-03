using Dominio.Entidades.Viagens;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Viagens
{
    internal class ViagemPosicaoConfigurator : BaseEntityConfigurator<ViagemPosicao>
    {
        protected override void InternalConfigure(EntityTypeBuilder<ViagemPosicao> builder)
        {
            builder.Property(vp => vp.ViagemId).IsRequired();
            builder.Property(vp => vp.DataHora).IsRequired();
            builder.Property(vp => vp.Latitude).IsRequired();
            builder.Property(vp => vp.Longitude).IsRequired();

            builder.HasOne(vp => vp.Viagem).WithMany().HasForeignKey(vp => vp.ViagemId);

            builder.ToTable(nameof(ViagemPosicao));
        }
    }
}
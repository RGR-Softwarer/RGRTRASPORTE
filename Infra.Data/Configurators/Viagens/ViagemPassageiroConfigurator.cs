using Dominio.Entidades.Viagens;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Viagens
{
    internal class ViagemPassageiroConfigurator : BaseEntityConfigurator<ViagemPassageiro>
    {
        protected override void InternalConfigure(EntityTypeBuilder<ViagemPassageiro> builder)
        {
            builder.Property(vp => vp.ViagemId).IsRequired();
            builder.Property(vp => vp.PassageiroId).IsRequired();

            builder.HasOne(vp => vp.Viagem).WithMany().HasForeignKey(vp => vp.ViagemId);
            builder.HasOne(vp => vp.Passageiro).WithMany().HasForeignKey(vp => vp.PassageiroId);

            builder.ToTable(nameof(ViagemPassageiro));
        }
    }
}

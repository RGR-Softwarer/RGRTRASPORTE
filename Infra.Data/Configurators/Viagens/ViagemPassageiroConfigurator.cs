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
            prefixo = "VIP";

            builder.ToTable("T_VIAGEM_PASSAGEIRO");

            builder.Property(vp => vp.ViagemId)
                .IsRequired()
                .HasColumnName($"{prefixo}_VIAGEM_ID");

            builder.Property(vp => vp.PassageiroId)
                .IsRequired()
                .HasColumnName($"{prefixo}_PASSAGEIRO_ID");

            builder.HasOne(vp => vp.Viagem)
                .WithMany()
                .HasForeignKey(vp => vp.ViagemId);

            builder.HasOne(vp => vp.Passageiro)
                .WithMany()
                .HasForeignKey(vp => vp.PassageiroId);
        }
    }
}

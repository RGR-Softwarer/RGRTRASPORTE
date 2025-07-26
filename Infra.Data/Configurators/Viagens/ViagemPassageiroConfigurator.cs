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
                .HasColumnName($"VIA_ID");

            builder.Property(vp => vp.PassageiroId)
                .IsRequired()
                .HasColumnName($"PAS_ID");

            builder.Property(vp => vp.DataReserva)
                .IsRequired()
                .HasColumnName($"{prefixo}_DATA_RESERVA");

            builder.Property(vp => vp.Confirmado)
                .IsRequired()
                .HasColumnName($"{prefixo}_CONFIRMADO");

            builder.Property(vp => vp.DataConfirmacao)
                .HasColumnName($"{prefixo}_DATA_CONFIRMACAO");

            builder.Property(vp => vp.Observacao)
                .HasColumnName($"{prefixo}_OBSERVACAO");

            builder.HasOne(vp => vp.Viagem)
                .WithMany(v => v.Passageiros)
                .HasForeignKey(vp => vp.ViagemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(vp => vp.Passageiro)
                .WithMany()
                .HasForeignKey(vp => vp.PassageiroId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

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
            prefixo = "VPO";

            builder.ToTable("T_VIAGEM_POSICAO");

            builder.Property(vp => vp.ViagemId)
                .IsRequired()
                .HasColumnName($"VIA_VIAGEM_ID");

            builder.Property(vp => vp.DataHora)
                .IsRequired()
                .HasColumnName($"{prefixo}_DATA_HORA");

            builder.Property(vp => vp.Latitude)
                .IsRequired()
                .HasColumnName($"{prefixo}_LATITUDE");

            builder.Property(vp => vp.Longitude)
                .IsRequired()
                .HasColumnName($"{prefixo}_LONGITUDE");

            builder.HasOne(vp => vp.Viagem)
                .WithMany()
                .HasForeignKey(vp => vp.ViagemId);
        }
    }
}

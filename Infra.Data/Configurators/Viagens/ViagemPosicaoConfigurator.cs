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
                .HasColumnName($"VIA_ID");

            builder.Property(vp => vp.DataHora)
                .IsRequired()
                .HasColumnName($"{prefixo}_DATA_HORA");

            // Configurar o mapeamento do Value Object Coordenada usando owned entity
            builder.OwnsOne(vp => vp.Coordenada, coord =>
            {
                coord.Property(c => c.Latitude)
                    .HasColumnName($"{prefixo}_LATITUDE")
                    .IsRequired();
                
                coord.Property(c => c.Longitude)
                    .HasColumnName($"{prefixo}_LONGITUDE")
                    .IsRequired();
            });

            // Ignorar as propriedades de conveniência somente leitura
            builder.Ignore(vp => vp.Latitude);
            builder.Ignore(vp => vp.Longitude);

            builder.HasOne(vp => vp.Viagem)
                .WithMany(v => v.Posicoes)
                .HasForeignKey(vp => vp.ViagemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

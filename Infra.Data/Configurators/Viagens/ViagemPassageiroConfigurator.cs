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

            builder.Property(vp => vp.StatusConfirmacao)
                .IsRequired()
                .HasColumnName($"{prefixo}_STATUS_CONFIRMACAO");

            builder.Property(vp => vp.DataLimiteConfirmacao)
                .HasColumnName($"{prefixo}_DATA_LIMITE_CONFIRMACAO");

            builder.Property(vp => vp.PassageiroFixo)
                .IsRequired()
                .HasColumnName($"{prefixo}_PASSAGEIRO_FIXO");

            builder.HasOne(vp => vp.Viagem)
                .WithMany(v => v.Passageiros)
                .HasForeignKey(vp => vp.ViagemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices para melhorar performance de consultas
            builder.HasIndex(vp => vp.ViagemId)
                .HasDatabaseName("IX_T_VIAGEM_PASSAGEIRO_VIA_ID");

            builder.HasIndex(vp => vp.PassageiroId)
                .HasDatabaseName("IX_T_VIAGEM_PASSAGEIRO_PAS_ID");

            builder.HasIndex(vp => vp.StatusConfirmacao)
                .HasDatabaseName("IX_T_VIAGEM_PASSAGEIRO_STATUS_CONFIRMACAO");

            builder.HasIndex(vp => vp.Confirmado)
                .HasDatabaseName("IX_T_VIAGEM_PASSAGEIRO_CONFIRMADO");

            builder.HasIndex(vp => new { vp.ViagemId, vp.PassageiroId })
                .IsUnique()
                .HasDatabaseName("IX_T_VIAGEM_PASSAGEIRO_VIA_PAS_UNIQUE");

            // Ignorar navegação para entidade do CadastroContext
            builder.Ignore(vp => vp.Passageiro);
        }
    }
}

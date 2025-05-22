using Dominio.Entidades.Auditoria;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Auditoria
{
    internal class HistoricoPropriedadeConfigurator : BaseEntityConfigurator<HistoricoPropriedade>
    {
        protected override void InternalConfigure(EntityTypeBuilder<HistoricoPropriedade> builder)
        {
            prefixo = "HPR";

            builder.ToTable("T_HISTORICO_PROPRIEDADE");

            builder.Property(h => h.Propriedade)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName($"{prefixo}_PROPRIEDADE");

            builder.Property(h => h.De)
                .HasMaxLength(1000)
                .HasColumnName($"{prefixo}_DE");

            builder.Property(h => h.Para)
                .HasMaxLength(1000)
                .HasColumnName($"{prefixo}_PARA");

            builder.Property(h => h.HistoricoObjetoId)
                .IsRequired()
                .HasColumnName($"{prefixo}_HISTORICO_OBJETO_ID");

            builder.HasOne(h => h.HistoricoObjeto)
                .WithMany(p => p.HistoricoPropriedade)
                .HasForeignKey(h => h.HistoricoObjetoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
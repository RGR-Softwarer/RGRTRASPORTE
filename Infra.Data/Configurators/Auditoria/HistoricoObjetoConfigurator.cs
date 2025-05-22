using Dominio.Entidades.Auditoria;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Auditoria
{
    internal class HistoricoObjetoConfigurator : BaseEntityConfigurator<HistoricoObjeto>
    {
        protected override void InternalConfigure(EntityTypeBuilder<HistoricoObjeto> builder)
        {
            prefixo = "HOB";

            builder.ToTable("T_HISTORICO_OBJETO");

            builder.Property(h => h.Objeto)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName($"{prefixo}_OBJETO");

            builder.Property(h => h.DescricaoObjeto)
                .IsRequired()
                .HasMaxLength(1000)
                .HasColumnName($"{prefixo}_DESCRICAO_OBJETO");

            builder.Property(h => h.DescricaoAcao)
                .HasMaxLength(500)
                .HasColumnName($"{prefixo}_DESCRICAO_ACAO");

            builder.Property(h => h.IP)
                .HasMaxLength(50)
                .HasColumnName($"{prefixo}_IP");

            builder.Property(h => h.Acao)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName($"{prefixo}_ACAO");

            builder.Property(h => h.TipoAuditado)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName($"{prefixo}_TIPO_AUDITADO");

            builder.Property(h => h.OrigemAuditado)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName($"{prefixo}_ORIGEM_AUDITADO");

            builder.Property(h => h.CodigoObjeto)
                .HasColumnName($"{prefixo}_CODIGO_OBJETO");

            builder.Property(h => h.Data)
                .HasColumnName($"{prefixo}_DATA");
        }
    }
}

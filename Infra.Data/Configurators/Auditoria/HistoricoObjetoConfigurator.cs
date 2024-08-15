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
            builder.Property(h => h.Objeto).IsRequired().HasMaxLength(255);
            builder.Property(h => h.DescricaoObjeto).IsRequired().HasMaxLength(1000);
            builder.Property(h => h.DescricaoAcao).HasMaxLength(500);
            builder.Property(h => h.IP).HasMaxLength(50);
            builder.Property(h => h.Acao).IsRequired().HasConversion<int>();
            builder.Property(h => h.TipoAuditado).IsRequired().HasConversion<int>();
            builder.Property(h => h.OrigemAuditado).IsRequired().HasConversion<int>();
            builder.Property(h => h.CodigoObjeto);
            builder.Property(h => h.Data);

            builder.ToTable(nameof(HistoricoObjeto));
        }
    }
}
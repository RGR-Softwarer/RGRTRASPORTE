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

            builder.Property(h => h.Propriedade).IsRequired().HasMaxLength(255);
            builder.Property(h => h.De).IsRequired(false).HasMaxLength(1000);
            builder.Property(h => h.Para).IsRequired(false).HasMaxLength(1000);
            builder.Property(h => h.HistoricoObjetoId).IsRequired();
            builder.HasOne(h => h.HistoricoObjeto).WithMany(p => p.HistoricoPropriedade).HasForeignKey(h => h.HistoricoObjetoId).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(nameof(HistoricoPropriedade));
        }
    }
}

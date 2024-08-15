using Dominio.Enums;

namespace Dominio.Entidades.Auditoria
{
    public class HistoricoObjeto : BaseEntity
    {
        public string Objeto { get; set; }

        public long CodigoObjeto { get; set; }

        public string DescricaoObjeto { get; set; }

        public AcaoBancoDadosEnum Acao { get; set; }

        public TipoAuditadoEnum TipoAuditado { get; set; }

        public OrigemAuditadoEnum OrigemAuditado { get; set; }

        public string DescricaoAcao { get; set; }

        public DateTime Data { get; set; }

        public string IP { get; set; }

        public virtual ICollection<HistoricoPropriedade> HistoricoPropriedade { get; set; }

        public string Auditado
        {
            get
            {
                if (TipoAuditado == TipoAuditadoEnum.Usuario)
                    return "";
                if (TipoAuditado == TipoAuditadoEnum.Integradoras)
                    return " (Via Integração -> " + IP + ")";
                if (TipoAuditado == TipoAuditadoEnum.Sistema)
                    return "Sistema";
                else
                    return "";
            }
        }
    }
}
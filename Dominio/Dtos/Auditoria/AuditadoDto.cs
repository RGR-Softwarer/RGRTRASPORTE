using Dominio.Enums.Auditoria;

namespace Dominio.Dtos.Auditoria
{
    public class AuditadoDto
    {
        public string IP { get; set; }

        public string Texto { get; set; }

        public TipoAuditadoEnum TipoAuditado { get; set; }

        public OrigemAuditadoEnum OrigemAuditado { get; set; }

    }
}

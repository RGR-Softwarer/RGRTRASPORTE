using Dominio.Enums.Auditoria;

namespace Application.Dtos.Auditoria
{
    public class AuditadoDto
    {
        public string IP { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public TipoAuditadoEnum TipoAuditado { get; set; }
        public OrigemAuditadoEnum OrigemAuditado { get; set; }
    }
} 
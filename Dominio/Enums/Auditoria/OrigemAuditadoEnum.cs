namespace Dominio.Enums.Auditoria
{
    public enum OrigemAuditadoEnum
    {
        Sistema = 0,
        Integradoras = 1,
    }
    public static class OrigemAuditadoHelper
    {
        public static string ObterDescricao(this OrigemAuditadoEnum acao)
        {
            switch (acao)
            {
                case OrigemAuditadoEnum.Sistema: return "Sistema";
                case OrigemAuditadoEnum.Integradoras: return "Integradoras";
                default: return "Não mapeado";
            }
        }
    }
}

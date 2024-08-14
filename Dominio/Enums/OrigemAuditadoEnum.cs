namespace Dominio.Enums
{
    public enum OrigemAuditadoEnum
    {
        Sistema = 0,
    }
    public static class OrigemAuditadoHelper
    {
        public static string ObterDescricao(this OrigemAuditadoEnum acao)
        {
            switch (acao)
            {
                case OrigemAuditadoEnum.Sistema: return "Sistema";                
                default: return "Não mapeado";
            }
        }
    }
}

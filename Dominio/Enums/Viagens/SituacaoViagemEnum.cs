namespace Dominio.Enums.Viagens
{
    public enum SituacaoViagemEnum
    {
        Agendada = 1,
        EmAndamento = 2,
        Finalizada = 3,
        Cancelada = 4
    }

    public static class SituacaoViagemEnumHelper
    {
        public static string ObterDescricao(this SituacaoViagemEnum situacao)
        {
            switch (situacao)
            {
                case SituacaoViagemEnum.Agendada: return "Agendada";
                case SituacaoViagemEnum.EmAndamento: return "Em andamento";
                case SituacaoViagemEnum.Finalizada: return "Finalizada";
                case SituacaoViagemEnum.Cancelada: return "Cancelada";
                default: return string.Empty;
            }
        }
    }
}

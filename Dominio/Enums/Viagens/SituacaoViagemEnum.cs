namespace Dominio.Enums.Viagens
{
    public enum SituacaoViagemEnum
    {
        NaoIniciada = 0,
        EmTransito = 1,
        Finalizada = 2
    }

    public static class SituacaoViagemEnumHelper
    {
        public static string ObterDescricao(this SituacaoViagemEnum situacao)
        {
            switch (situacao)
            {
                case SituacaoViagemEnum.NaoIniciada: return "Não iniciada";
                case SituacaoViagemEnum.EmTransito: return "Em trânsito";
                case SituacaoViagemEnum.Finalizada: return "Finalizada";
                default: return string.Empty;
            }
        }
    }
}

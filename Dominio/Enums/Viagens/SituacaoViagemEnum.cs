namespace Dominio.Enums.Viagens
{
    public enum SituacaoViagemEnum
    {
        NaoIniciada = 1,
        EmTransito = 2,
        Finalizada = 3
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

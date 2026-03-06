namespace Dominio.Enums.Viagens
{
    public enum TipoTrechoViagemEnum
    {
        Ida = 1,    // Casa -> Faculdade
        Volta = 2   // Faculdade -> Casa
    }

    public static class TipoTrechoViagemEnumHelper
    {
        public static string ObterDescricao(this TipoTrechoViagemEnum tipoTrecho)
        {
            switch (tipoTrecho)
            {
                case TipoTrechoViagemEnum.Ida: return "Ida";
                case TipoTrechoViagemEnum.Volta: return "Volta";
                default: return string.Empty;
            }
        }
    }
}



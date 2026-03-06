namespace Dominio.Enums.Viagens
{
    public enum StatusConfirmacaoEnum
    {
        AguardandoConfirmacao = 1,
        Confirmado = 2,
        NaoVai = 3,
        Cancelado = 4
    }

    public static class StatusConfirmacaoEnumHelper
    {
        public static string ObterDescricao(this StatusConfirmacaoEnum status)
        {
            switch (status)
            {
                case StatusConfirmacaoEnum.AguardandoConfirmacao: return "Aguardando confirmação";
                case StatusConfirmacaoEnum.Confirmado: return "Confirmado";
                case StatusConfirmacaoEnum.NaoVai: return "Não vai";
                case StatusConfirmacaoEnum.Cancelado: return "Cancelado";
                default: return string.Empty;
            }
        }
    }
}



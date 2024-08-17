namespace Dominio.Enums.Veiculo
{
    public enum TipoModeloVeiculoEnum
    {
        Onibus = 1,
        Van = 2,
        MicroOnibus = 3
    }

    public static class TipoModeloVeiculoHelper
    {
        public static string ObterDescricao(this TipoModeloVeiculoEnum tipoModeloVeiculo)
        {
            switch (tipoModeloVeiculo)
            {
                case TipoModeloVeiculoEnum.Onibus: return "Ônibus";
                case TipoModeloVeiculoEnum.Van: return "Van";
                case TipoModeloVeiculoEnum.MicroOnibus: return "Micro-ônibus";
                default: return string.Empty;
            }
        }
    }
}

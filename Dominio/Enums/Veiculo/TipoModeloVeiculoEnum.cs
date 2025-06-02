using System.ComponentModel;

namespace Dominio.Enums.Veiculo
{
    public enum TipoModeloVeiculoEnum
    {
        [Description("Ônibus")]
        Onibus = 0,
        
        [Description("Micro-ônibus")]
        MicroOnibus = 1,
        
        [Description("Van")]
        Van = 2,
        
        [Description("Carro")]
        Carro = 3
    }

    public static class TipoModeloVeiculoHelper
    {
        public static string ObterDescricao(this TipoModeloVeiculoEnum tipoModeloVeiculo)
        {
            return tipoModeloVeiculo switch
            {
                TipoModeloVeiculoEnum.Onibus => "Ônibus",
                TipoModeloVeiculoEnum.MicroOnibus => "Micro-ônibus",
                TipoModeloVeiculoEnum.Van => "Van",
                TipoModeloVeiculoEnum.Carro => "Carro",
                _ => string.Empty
            };
        }

        public static string ObterDescricaoSituacao(bool situacao)
        {
            return situacao ? "Ativo" : "Inativo";
        }

        public static string ObterDescricaoBoolean(bool valor, string textoTrue = "Sim", string textoFalse = "Não")
        {
            return valor ? textoTrue : textoFalse;
        }

        public static Dictionary<int, string> ObterTodosOsTipos()
        {
            return new Dictionary<int, string>
            {
                { (int)TipoModeloVeiculoEnum.Onibus, "Ônibus" },
                { (int)TipoModeloVeiculoEnum.MicroOnibus, "Micro-ônibus" },
                { (int)TipoModeloVeiculoEnum.Van, "Van" },
                { (int)TipoModeloVeiculoEnum.Carro, "Carro" }
            };
        }
    }
}

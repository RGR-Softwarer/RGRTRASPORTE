using Dominio.Enums;

namespace Dominio.Entidades.Veiculos
{
    public class Veiculo : BaseEntity
    {
        public string Placa { get; }
        public string Modelo { get; }
        public string Marca { get; }
        public int Ano { get; }
        public string Cor { get; }
        public string Renavam { get; }
        public TipoCombustivelEnum TipoCombustivel { get; }
        public TipoVeiculoEnum TipoVeiculo { get; }
        public string Capacidade { get; }
        public CategoriaCNHEnum CategoriaCNH { get; }
        public StatusVeiculoEnum Status { get; }
        public string Observacao { get; }
        protected override string Descricao => Id.ToString();
    }
}

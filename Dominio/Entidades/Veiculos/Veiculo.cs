using Dominio.Enums;

namespace Dominio.Entidades.Veiculos
{
    public class Veiculo : BaseEntity
    {
        public string Placa { get; private set; }
        public string Modelo { get; private set; }
        public string Marca { get; private set; }
        public int Ano { get; private set; }
        public string Cor { get; private set; }
        public string Renavam { get; private set; }
        public TipoCombustivelEnum TipoCombustivel { get; private set; }
        public TipoVeiculoEnum TipoVeiculo { get; private set; }
        public string Capacidade { get; private set; }
        public CategoriaCNHEnum CategoriaCNH { get; private set; }
        public StatusVeiculoEnum Status { get; private set; }
        public string Observacao { get; private set; }
    }
}

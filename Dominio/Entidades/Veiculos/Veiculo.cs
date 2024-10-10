using Dominio.Enums.Veiculo;

namespace Dominio.Entidades.Veiculos
{
    public class Veiculo : BaseEntity
    {
        public string Placa { get; private set; }
        public string Modelo { get; private set; }
        public string Marca { get; private set; }
        public string NumeroChassi { get; private set; }
        public int AnoModelo { get; private set; }
        public int AnoFabricacao { get; private set; }
        public string Cor { get; private set; }
        public string Renavam { get; private set; }
        public virtual DateTime? VencimentoLicenciamento { get; private set; }
        public TipoCombustivelEnum TipoCombustivel { get; private set; }
        public StatusVeiculoEnum Status { get; private set; }
        public string Observacao { get; private set; }
        public long? ModeloVeiculoId { get; private set; }
        public virtual ModeloVeicular ModeloVeiculo { get; private set; }

        public virtual string PlacaFormatada
        {
            get { return string.IsNullOrWhiteSpace(Placa) ? string.Empty : $"{Placa.Substring(0, 3)}-{Placa.Substring(3, 4)}"; }
        }

        protected override string DescricaoFormatada => Placa;

    }
}

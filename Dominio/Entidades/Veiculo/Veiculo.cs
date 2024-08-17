using Dominio.Enums.Veiculo;

namespace Dominio.Entidades.Veiculo
{
    public class Veiculo : BaseEntity
    {
        public string Placa { get; }
        public string Modelo { get; }
        public string Marca { get; }
        public string NumeroChassi { get; }
        public int AnoModelo { get; }
        public int AnoFabricacao { get; }
        public string Cor { get; }
        public string Renavam { get; }
        public virtual DateTime? VencimentoLicenciamento { get; }
        public TipoCombustivelEnum TipoCombustivel { get; }
        public StatusVeiculoEnum Status { get; }
        public string Observacao { get; }
        public long? ModeloVeiculoId { get; }
        public virtual ModeloVeicular ModeloVeiculo { get; set; }

        public virtual string PlacaFormatada
        {
            get { return string.IsNullOrWhiteSpace(Placa) ? string.Empty : $"{Placa.Substring(0, 3)}-{Placa.Substring(3, 4)}"; }
        }

        protected override string Descricao => Placa;

    }
}

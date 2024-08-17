using Dominio.Enums.Veiculo;

namespace Dominio.Dtos.Veiculo
{
    public class VeiculoDto
    {
        public long Id { get; set; }
        public string Placa { get; init; }
        public string Modelo { get; init; }
        public string Marca { get; init; }
        public string NumeroChassi { get; init; }
        public int AnoModelo { get; init; }
        public int AnoFabricacao { get; init; }
        public string Cor { get; init; }
        public string Renavam { get; init; }

        public DateTime? VencimentoLicenciamento { get; init; }

        public TipoCombustivelEnum TipoCombustivel { get; init; }
        public TipoVeiculoEnum TipoVeiculo { get; init; }
        public CategoriaCNHEnum CategoriaCNH { get; init; }
        public StatusVeiculoEnum Status { get; init; }

        public string Observacao { get; init; }
        public long? ModeloVeiculoId { get; init; }
    }
}

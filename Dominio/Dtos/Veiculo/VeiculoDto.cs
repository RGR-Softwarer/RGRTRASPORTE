using Dominio.Enums.Veiculo;

namespace Dominio.Dtos.Veiculo
{
    public class VeiculoDto
    {
        public long Id { get; set; }
        public string Placa { get; init; } = null!;
        public string Modelo { get; init; } = null!;
        public string Marca { get; init; } = null!;
        public string NumeroChassi { get; init; } = null!;
        public int AnoModelo { get; init; }
        public int AnoFabricacao { get; init; }
        public string Cor { get; init; } = null!;
        public string Renavam { get; init; } = null!;
        public DateTime? VencimentoLicenciamento { get; init; }
        public TipoCombustivelEnum TipoCombustivel { get; init; }
        public StatusVeiculoEnum Status { get; init; }
        public string Observacao { get; init; } = null!;
        public long? ModeloVeiculoId { get; init; }
        public string PlacaFormatada { get; init; } = null!;
    }
}

#nullable enable

using Dominio.Enums.Viagens;

namespace Dominio.Dtos.Viagens
{
    public class ViagemDto
    {
        public long Id { get; init; }
        public string Codigo { get; init; } = string.Empty;
        public long VeiculoId { get; init; }
        public long MotoristaId { get; init; }
        public long LocalidadeOrigemId { get; init; }
        public long LocalidadeDestinoId { get; init; }
        
        // Propriedades do PeriodoViagem
        public DateTime DataViagem { get; init; }
        public TimeSpan HorarioSaida { get; init; }
        public TimeSpan HorarioChegada { get; init; }
        
        public SituacaoViagemEnum Situacao { get; init; }
        public bool Lotado { get; init; }
        public DateTime? DataInicioViagem { get; init; }
        public DateTime? DataFimViagem { get; init; }
        public int QuantidadeVagas { get; init; }
        public int VagasDisponiveis { get; init; }
        
        // Propriedades dos Value Objects como primitivos para serialização
        public decimal DistanciaQuilometros { get; init; }
        public string DescricaoViagem { get; init; } = string.Empty;
        public string PolilinhaRota { get; init; } = string.Empty;
        
        public bool Ativo { get; init; }
        public long? GatilhoViagemId { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
        public string CreatedBy { get; init; } = string.Empty;
        public string UpdatedBy { get; init; } = string.Empty;
    }
}

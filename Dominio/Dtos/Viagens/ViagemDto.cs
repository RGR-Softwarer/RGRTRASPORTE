#nullable enable

using Dominio.Enums.Viagens;

namespace Dominio.Dtos.Viagens
{
    public class ViagemDto
    {
        public long Id { get; init; }
        public string Codigo { get; init; }
        public DateTime DataViagem { get; init; }
        public TimeSpan HorarioSaida { get; init; }
        public TimeSpan HorarioChegada { get; init; }
        public long VeiculoId { get; init; }
        public long MotoristaId { get; init; }
        public long LocalidadeOrigemId { get; init; }
        public long LocalidadeDestinoId { get; init; }
        public SituacaoViagemEnum Situacao { get; init; }
        public bool Lotado { get; init; }
        public DateTime? DataInicioViagem { get; init; }
        public DateTime? DataFimViagem { get; init; }
        public decimal ValorPassagem { get; init; }
        public int QuantidadeVagas { get; init; }
        public int VagasDisponiveis { get; init; }
        public decimal Distancia { get; init; }
        public string DescricaoViagem { get; init; }
        public string PolilinhaRota { get; init; }
        public bool Ativo { get; init; }
        public long? GatilhoViagemId { get; init; }
    }
}
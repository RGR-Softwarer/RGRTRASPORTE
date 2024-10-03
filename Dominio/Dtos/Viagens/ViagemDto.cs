using Dominio.Enums.Viagens;

namespace Dominio.Dtos.Viagens
{
    public class ViagemDto
    {
        public long Id { get; set; }
        public string CodigoViagem { get; init; }
        public DateTime DataViagem { get; init; }
        public long VeiculoId { get; init; }
        public long MotoristaId { get; init; }
        public long OrigemId { get; init; }
        public long DestinoId { get; init; }
        public long? GatinhoViagemId { get; init; }
        public DateTime HorarioSaida { get; init; }
        public DateTime HorarioChegada { get; init; }
        public SituacaoViagemEnum Situacao { get; init; }
        public string MotivoProblema { get; init; }
        public string DescricaoViagem { get; init; }
        public DateTime? DataInicioViagem { get; init; }
        public decimal? LatitudeInicioViagem { get; init; }
        public decimal? LongitudeInicioViagem { get; init; }
        public DateTime? DataFimViagem { get; init; }
        public decimal? LatitudeFimViagem { get; init; }
        public decimal? LongitudeFimViagem { get; init; }
        public decimal Distancia { get; init; }
        public decimal? DistanciaRealizada { get; init; }
        public string PolilinhaRota { get; init; }
        public string? PolilinhaRotaRealizada { get; init; }
        public int NumeroPassageiros { get; init; }
        public bool Lotado { get; init; }
        public bool Excesso { get; init; }
    }
}
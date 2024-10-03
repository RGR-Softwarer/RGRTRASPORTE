using Dominio.Enums.Data;

namespace Dominio.Dtos.Viagens.Gatilho
{
    public class GatilhoViagemDto
    {
        public long Id { get; set; }
        public string Descricao { get; init; }
        public long VeiculoId { get; init; }
        public long MotoristaId { get; init; }
        public long OrigemId { get; init; }
        public long DestinoId { get; init; }
        public DateTime HorarioSaida { get; init; }
        public DateTime HorarioChegada { get; init; }
        public string DescricaoViagem { get; init; }
        public decimal Distancia { get; init; }
        public string PolilinhaRota { get; init; }
        public bool Ativo { get; init; }
        public List<DiaSemanaEnum> DiasSemana { get; init; }
    }
}

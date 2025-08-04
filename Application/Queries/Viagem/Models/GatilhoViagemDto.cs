using Dominio.Enums.Data;

namespace Application.Queries.Viagem.Models
{
    public class GatilhoViagemDto
    {
        public long Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public long VeiculoId { get; set; }
        public long MotoristaId { get; set; }
        public long OrigemId { get; set; }
        public long DestinoId { get; set; }
        public DateTime HorarioSaida { get; set; }
        public DateTime HorarioChegada { get; set; }
        public string DescricaoViagem { get; set; } = string.Empty;
        public decimal Distancia { get; set; }
        public string PolilinhaRota { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public List<DiaSemanaEnum> DiasSemana { get; set; } = new();
    }
} 
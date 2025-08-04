using Application.Dtos.Viagens;

namespace Application.Dtos.Viagens
{
    public class ViagemFinalizadaJobData : ViagemJobDataBase
    {
        public DateTime DataFim { get; set; }
        public string LocalFim { get; set; } = string.Empty;
        public decimal DistanciaPercorrida { get; set; }
    }
} 
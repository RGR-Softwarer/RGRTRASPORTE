using Application.Dtos.Viagens;

namespace Application.Dtos.Viagens
{
    public class ViagemCriadaJobData : ViagemJobDataBase
    {
        public string CodigoViagem { get; set; } = string.Empty;
        public long VeiculoId { get; set; }
        public long MotoristaId { get; set; }
        public DateTime DataViagem { get; set; }
        public decimal ValorViagem { get; set; }
    }
} 
using Application.Dtos.Viagens;

namespace Application.Dtos.Viagens
{
    public class ViagemAtualizadaJobData : ViagemJobDataBase
    {
        public string CodigoViagem { get; set; } = string.Empty;
        public string CampoAlterado { get; set; } = string.Empty;
        public string ValorAnterior { get; set; } = string.Empty;
        public string ValorNovo { get; set; } = string.Empty;
        public DateTime DataAlteracao { get; set; }
    }
} 
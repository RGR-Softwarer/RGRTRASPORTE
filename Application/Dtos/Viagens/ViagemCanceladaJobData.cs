using Application.Dtos.Viagens;

namespace Application.Dtos.Viagens
{
    public class ViagemCanceladaJobData : ViagemJobDataBase
    {
        public string MotivoCancelamento { get; set; } = string.Empty;
        public DateTime DataCancelamento { get; set; }
    }
} 
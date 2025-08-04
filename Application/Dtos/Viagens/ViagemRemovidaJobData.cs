using Application.Dtos.Viagens;

namespace Application.Dtos.Viagens
{
    public class ViagemRemovidaJobData : ViagemJobDataBase
    {
        public string MotivoRemocao { get; set; } = string.Empty;
        public DateTime DataRemocao { get; set; }
    }
} 
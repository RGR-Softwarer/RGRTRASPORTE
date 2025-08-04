using Application.Dtos.Viagens;

namespace Application.Dtos.Viagens
{
    public class ViagemIniciadaJobData : ViagemJobDataBase
    {
        public DateTime DataInicio { get; set; }
        public string LocalInicio { get; set; } = string.Empty;
    }
} 
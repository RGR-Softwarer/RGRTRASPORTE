namespace Application.Dtos.Viagens
{
    public abstract class ViagemJobDataBase
    {
        public long ViagemId { get; set; }
        public DateTime DataProcessamento { get; set; } = DateTime.UtcNow;
    }
} 
namespace Dominio.Dtos.Viagens
{
    public class ViagemPassageiroDto
    {
        public long Id { get; init; }
        public long ViagemId { get; init; }
        public long PassageiroId { get; init; }
        public DateTime DataReserva { get; init; }
        public bool Confirmado { get; init; }
        public DateTime? DataConfirmacao { get; init; }
    }
}

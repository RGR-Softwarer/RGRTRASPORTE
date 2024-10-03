namespace Dominio.Dtos.Viagens
{
    public class ViagemPassageiroDto
    {
        public long Id { get; set; }
        public long ViagemId { get; init; }
        public long PassageiroId { get; init; }
    }
}
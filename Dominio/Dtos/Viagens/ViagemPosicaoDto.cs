namespace Dominio.Dtos.Viagens
{
    public class ViagemPosicaoDto
    {
        public long Id { get; set; }
        public long ViagemId { get; init; }
        public DateTime DataHora { get; init; }
        public decimal Latitude { get; init; }
        public decimal Longitude { get; init; }
    }
}

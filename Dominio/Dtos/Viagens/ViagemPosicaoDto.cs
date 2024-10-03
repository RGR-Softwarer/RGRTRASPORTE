namespace Dominio.Dtos.Viagens
{
    public class ViagemPosicaoDto
    {
        public long Id { get; set; }
        public long ViagemId { get; init; }
        public DateTime DataHora { get; init; }
        public string Latitude { get; init; }
        public string Longitude { get; init; }
    }
}
namespace Application.Queries.Viagem.Models;

public class ViagemPosicaoDto
{
    public long Id { get; set; }
    public long ViagemId { get; set; }
    public DateTime DataHora { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
} 
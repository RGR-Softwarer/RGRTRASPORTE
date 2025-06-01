namespace Application.Queries.Viagem.Models;

public class ViagemPassageiroDto
{
    public long Id { get; set; }
    public long ViagemId { get; set; }
    public long PassageiroId { get; set; }
    public string NomePassageiro { get; set; }
    public string CPFPassageiro { get; set; }
    public bool Ativo { get; set; }
} 
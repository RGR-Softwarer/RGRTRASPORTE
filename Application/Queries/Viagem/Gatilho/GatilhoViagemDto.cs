namespace Application.Queries.Viagem.Gatilho;

public class GatilhoViagemDto
{
    public long Id { get; set; }
    public long ViagemId { get; set; }
    public string Descricao { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
} 
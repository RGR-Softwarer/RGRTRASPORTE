namespace Application.Queries.Viagem.Models;

public class ViagemDto
{
    public Guid Id { get; set; }
    public DateTime DataViagem { get; set; }
    public string Origem { get; set; }
    public string Destino { get; set; }
    public decimal ValorViagem { get; set; }
    public string Status { get; set; }
    
    public VeiculoViagemDto Veiculo { get; set; }
    public MotoristaViagemDto Motorista { get; set; }
    public List<PassageiroViagemDto> Passageiros { get; set; }
}

public class VeiculoViagemDto
{
    public Guid Id { get; set; }
    public string Placa { get; set; }
    public string Modelo { get; set; }
}

public class MotoristaViagemDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Documento { get; set; }
}

public class PassageiroViagemDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Documento { get; set; }
} 

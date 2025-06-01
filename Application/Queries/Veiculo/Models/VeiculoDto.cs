namespace Application.Queries.Veiculo.Models;

public class VeiculoDto
{
    public long Id { get; set; }
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public int Ano { get; set; }
    public int Capacidade { get; set; }
    public string Cor { get; set; }
    public string Chassi { get; set; }
    public string Renavam { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
}

public class VeiculoListagemDto
{
    public long Id { get; set; }
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public int Ano { get; set; }
    public int Capacidade { get; set; }
    public bool Ativo { get; set; }
} 
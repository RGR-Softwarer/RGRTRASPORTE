namespace Application.Queries.Localidade.Models;

public class LocalidadeDto
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Cep { get; set; }
    public string Logradouro { get; set; }
    public string Numero { get; set; }
    public string Complemento { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string Pais { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
}

public class LocalidadeListagemDto
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public bool Ativo { get; set; }
} 
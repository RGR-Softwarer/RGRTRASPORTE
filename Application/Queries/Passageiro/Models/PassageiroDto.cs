namespace Application.Queries.Passageiro.Models;

public class PassageiroDto
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Documento { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Endereco { get; set; }
    public DateTime DataCadastro { get; set; }
    public bool Ativo { get; set; }
}

public class PassageiroListagemDto
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Documento { get; set; }
    public string Telefone { get; set; }
    public bool Ativo { get; set; }
} 

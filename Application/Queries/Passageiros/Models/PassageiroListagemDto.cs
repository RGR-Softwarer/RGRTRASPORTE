namespace Application.Queries.Passageiros.Models;

// DTO otimizado para listagem (apenas campos essenciais)
public class PassageiroListagemDto
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string CPFFormatado { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public bool Situacao { get; set; }
    public string SituacaoDescricao { get; set; } = string.Empty;
    public bool Ativo { get; set; }
} 
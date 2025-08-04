using Dominio.Enums.Veiculo;

namespace Application.Queries.Veiculo.ModeloVeicular.Models;

// DTO otimizado para listagem (apenas campos essenciais)
public class ModeloVeicularListagemDto
{
    public long Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public TipoModeloVeiculoEnum Tipo { get; set; }
    public string TipoDescricao { get; set; } = string.Empty;
    public int QuantidadeAssento { get; set; }
    public bool Situacao { get; set; }
    public string SituacaoDescricao { get; set; } = string.Empty;
    public bool Ativo { get; set; }
} 
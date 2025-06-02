using Dominio.Enums.Veiculo;

namespace Application.Queries.Veiculo.ModeloVeicular.Models;

public class ModeloVeicularDto
{
    public long Id { get; set; }
    public bool Situacao { get; set; }
    public string SituacaoDescricao { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string DescricaoModelo { get; set; } = string.Empty;
    public TipoModeloVeiculoEnum Tipo { get; set; }
    public string TipoDescricao { get; set; } = string.Empty;
    public int QuantidadeAssento { get; set; }
    public int QuantidadeEixo { get; set; }
    public int CapacidadeMaxima { get; set; }
    public int PassageirosEmPe { get; set; }
    public bool PossuiBanheiro { get; set; }
    public string PossuiBanheiroDescricao { get; set; } = string.Empty;
    public bool PossuiClimatizador { get; set; }
    public string PossuiClimatizadorDescricao { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
} 
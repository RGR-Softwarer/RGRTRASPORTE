using Dominio.Enums.Veiculo;

namespace Application.Queries.Veiculo.ModeloVeicular.Models;

public class ModeloVeicularDto
{
    public long Id { get; set; }
    public bool Situacao { get; set; }
    public string DescricaoModelo { get; set; }
    public TipoModeloVeiculoEnum Tipo { get; set; }
    public int QuantidadeAssento { get; set; }
    public int QuantidadeEixo { get; set; }
    public int CapacidadeMaxima { get; set; }
    public int PassageirosEmPe { get; set; }
    public bool PossuiBanheiro { get; set; }
    public bool PossuiClimatizador { get; set; }
} 
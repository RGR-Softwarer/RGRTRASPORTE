using Dominio.Enums.Viagens;

namespace Application.Queries.Viagem.Models;

public class ViagemDto
{
    public long Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public DateTime DataViagem { get; set; }
    public string Origem { get; set; } = string.Empty;
    public string Destino { get; set; } = string.Empty;
    public string LocalidadeOrigemNome { get; set; } = string.Empty;
    public string LocalidadeDestinoNome { get; set; } = string.Empty;
    public decimal ValorViagem { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Situacao { get; set; } = string.Empty;
    public TipoTrechoViagemEnum TipoTrecho { get; set; }
    public string TipoTrechoDescricao => TipoTrecho == TipoTrechoViagemEnum.Ida ? "Ida" : "Volta";
    public long? ViagemParId { get; set; }
    public int QuantidadeVagas { get; set; }
    public int VagasDisponiveis { get; set; }
    
    public VeiculoViagemDto? Veiculo { get; set; }
    public MotoristaViagemDto? Motorista { get; set; }
    public List<PassageiroViagemDto> Passageiros { get; set; } = new();
}

public class VeiculoViagemDto
{
    public long Id { get; set; }
    public string Placa { get; set; }
    public string Modelo { get; set; }
}

public class MotoristaViagemDto
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Documento { get; set; }
}

public class PassageiroViagemDto
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Documento { get; set; }
} 

using Dominio.Enums.Veiculo;

namespace Application.Queries.Veiculo.Models;

public class VeiculoDto
{
    public long Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string PlacaFormatada { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string NumeroChassi { get; set; } = string.Empty;
    public int AnoModelo { get; set; }
    public int AnoFabricacao { get; set; }
    public string Cor { get; set; } = string.Empty;
    public string Renavam { get; set; } = string.Empty;
    public DateTime? VencimentoLicenciamento { get; set; }
    public TipoCombustivelEnum TipoCombustivel { get; set; }
    public string TipoCombustivelDescricao { get; set; } = string.Empty;
    public StatusVeiculoEnum Status { get; set; }
    public string StatusDescricao { get; set; } = string.Empty;
    public string Observacao { get; set; } = string.Empty;
    public long? ModeloVeiculoId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int Capacidade { get; set; }
    public bool Ativo { get; set; }
}

// DTO otimizado para listagem (apenas campos essenciais)
public class VeiculoListagemDto
{
    public long Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Ano { get; set; }
    public int Capacidade { get; set; }
    public bool Ativo { get; set; }
}

// DTO otimizado para detalhes (todos os campos)
public class VeiculoDetalhesDto
{
    public long Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string PlacaFormatada { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string NumeroChassi { get; set; } = string.Empty;
    public int AnoModelo { get; set; }
    public int AnoFabricacao { get; set; }
    public string Cor { get; set; } = string.Empty;
    public string Renavam { get; set; } = string.Empty;
    public DateTime? VencimentoLicenciamento { get; set; }
    public TipoCombustivelEnum TipoCombustivel { get; set; }
    public string TipoCombustivelDescricao { get; set; } = string.Empty;
    public StatusVeiculoEnum Status { get; set; }
    public string StatusDescricao { get; set; } = string.Empty;
    public string Observacao { get; set; } = string.Empty;
    public long? ModeloVeiculoId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Capacidade { get; set; }
    public bool Ativo { get; set; }
} 

using Application.Common;
using Application.Queries.Base;
using Application.Queries.Veiculo.Models;
using Dominio.Enums.Veiculo;

namespace Application.Queries.Veiculo;

public class ObterVeiculosQuery : BaseQuery<BaseResponse<IEnumerable<VeiculoDto>>>
{
    public string? Placa { get; set; }
    public string? Modelo { get; set; }
    public string? Marca { get; set; }
    public StatusVeiculoEnum? Status { get; set; }
    public bool? Ativo { get; set; }

    public ObterVeiculosQuery()
    {
    }

    public ObterVeiculosQuery(
        string? placa = null,
        string? modelo = null,
        string? marca = null,
        StatusVeiculoEnum? status = null,
        bool? ativo = null)
    {
        Placa = placa;
        Modelo = modelo;
        Marca = marca;
        Status = status;
        Ativo = ativo;
    }
} 
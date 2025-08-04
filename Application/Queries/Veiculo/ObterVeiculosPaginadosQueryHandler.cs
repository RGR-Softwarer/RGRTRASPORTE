using Application.Common;
using Application.Queries.Base;
using Application.Queries.Veiculo.Models;
using AutoMapper;
using Dominio.Interfaces.Infra.Data;
using System.Linq.Expressions;

namespace Application.Queries.Veiculo;

public class ObterVeiculosPaginadosQueryHandler
    : QueryPaginadoHandlerBase<Dominio.Entidades.Veiculos.Veiculo, VeiculoDto, ObterVeiculosPaginadosQuery>
{
    public ObterVeiculosPaginadosQueryHandler(
        IGenericRepository<Dominio.Entidades.Veiculos.Veiculo> veiculoRepository,
        IMapper mapper) : base(veiculoRepository, mapper)
    {
    }

    protected override Expression<Func<Dominio.Entidades.Veiculos.Veiculo, bool>> BuildFilterExpression(List<FiltroGrid> filtros)
    {
        Expression<Func<Dominio.Entidades.Veiculos.Veiculo, bool>> expression = v => true;

        foreach (var filtro in filtros)
        {
            var valor = filtro.Valor.ToLower();

            expression = filtro.Campo.ToLower() switch
            {
                "placa" => CombineAnd(expression, v => v.Placa.Numero.ToLower().Contains(valor)),
                "placaformatada" => CombineAnd(expression, v => v.Placa.Numero.ToLower().Contains(valor)),
                "modelo" => CombineAnd(expression, v => v.Modelo.ToLower().Contains(valor)),
                "marca" => CombineAnd(expression, v => v.Marca.ToLower().Contains(valor)),
                "cor" => CombineAnd(expression, v => v.Cor.ToLower().Contains(valor)),
                "anomodelo" => CombineAnd(expression, v => v.AnoModelo.ToString().Contains(valor)),
                "anofabricacao" => CombineAnd(expression, v => v.AnoFabricacao.ToString().Contains(valor)),
                _ => expression
            };
        }

        return expression;
    }

    protected override string MapOrderField(string campoOrdenacao)
    {
        return campoOrdenacao.ToLower() switch
        {
            "placaformatada" => "Placa",
            "statusdescricao" => "Status",
            "tipocombustiveldescricao" => "TipoCombustivel",
            "createdat" => "CreatedAt",
            "updatedat" => "UpdatedAt",
            _ => "Id"
        };
    }

    private Expression<Func<Dominio.Entidades.Veiculos.Veiculo, bool>> CombineAnd(
        Expression<Func<Dominio.Entidades.Veiculos.Veiculo, bool>> left,
        Expression<Func<Dominio.Entidades.Veiculos.Veiculo, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(Dominio.Entidades.Veiculos.Veiculo), "v");

        var leftBody = new ParameterRebinder(parameter).Visit(left.Body);
        var rightBody = new ParameterRebinder(parameter).Visit(right.Body);

        var andExpression = Expression.AndAlso(leftBody, rightBody);

        return Expression.Lambda<Func<Dominio.Entidades.Veiculos.Veiculo, bool>>(andExpression, parameter);
    }
}

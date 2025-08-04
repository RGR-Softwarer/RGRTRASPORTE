using Application.Common;
using Application.Dtos;
using Application.Queries.Base;
using Application.Queries.Veiculo.ModeloVeicular.Models;
using Dominio.Entidades.Veiculos;
using Dominio.Interfaces.Infra.Data;
using Dominio.Enums.Veiculo;
using MediatR;
using System.Linq.Expressions;
using AutoMapper;

namespace Application.Queries.Veiculo.ModeloVeicular;

public class ObterModelosVeicularesPaginadosQueryHandler 
    : QueryPaginadoHandlerBase<Dominio.Entidades.Veiculos.ModeloVeicular, ModeloVeicularDto, ObterModelosVeicularesPaginadosQuery>
{
    public ObterModelosVeicularesPaginadosQueryHandler(
        IGenericRepository<Dominio.Entidades.Veiculos.ModeloVeicular> modeloVeicularRepository,
        IMapper mapper) : base(modeloVeicularRepository, mapper)
    {
    }

    protected override Expression<Func<Dominio.Entidades.Veiculos.ModeloVeicular, bool>> BuildFilterExpression(List<FiltroGrid> filtros)
    {
        Expression<Func<Dominio.Entidades.Veiculos.ModeloVeicular, bool>> expression = m => true;

        foreach (var filtro in filtros)
        {
            var valor = filtro.Valor.ToLower();

            expression = filtro.Campo.ToLower() switch
            {
                "descricao" => CombineAnd(expression, m => m.Descricao.ToLower().Contains(valor)),
                "tipo" => int.TryParse(filtro.Valor, out int tipo) 
                    ? CombineAnd(expression, m => m.Tipo == (TipoModeloVeiculoEnum)tipo) 
                    : expression,
                "situacao" => bool.TryParse(filtro.Valor, out bool situacao) 
                    ? CombineAnd(expression, m => m.Situacao == situacao) 
                    : expression,
                _ => expression
            };
        }

        return expression;
    }

    private Expression<Func<Dominio.Entidades.Veiculos.ModeloVeicular, bool>> CombineAnd(
        Expression<Func<Dominio.Entidades.Veiculos.ModeloVeicular, bool>> left,
        Expression<Func<Dominio.Entidades.Veiculos.ModeloVeicular, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(Dominio.Entidades.Veiculos.ModeloVeicular), "m");
        
        var leftBody = new ParameterRebinder(parameter).Visit(left.Body);
        var rightBody = new ParameterRebinder(parameter).Visit(right.Body);
        
        var andExpression = Expression.AndAlso(leftBody, rightBody);
        
        return Expression.Lambda<Func<Dominio.Entidades.Veiculos.ModeloVeicular, bool>>(andExpression, parameter);
    }

    protected override string MapOrderField(string campoOrdenacao)
    {
        return campoOrdenacao.ToLower() switch
        {
            "descricao" => "Descricao",
            "tipo" => "Tipo",
            "situacao" => "Situacao",
            "createdat" => "CreatedAt",
            "updatedat" => "UpdatedAt",
            _ => "Id"
        };
    }
} 

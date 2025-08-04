using Application.Common;
using Application.Queries.Base;
using Application.Queries.Passageiros.Models;
using AutoMapper;
using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Interfaces.Infra.Data;
using System.Linq.Expressions;

namespace Application.Queries.Passageiros;

public class ObterPassageirosPaginadosQueryHandler
    : QueryPaginadoHandlerBase<Passageiro, PassageiroDto, ObterPassageirosPaginadosQuery>
{
    public ObterPassageirosPaginadosQueryHandler(
        IGenericRepository<Passageiro> passageiroRepository,
        IMapper mapper) : base(passageiroRepository, mapper)
    {
    }

    protected override Expression<Func<Passageiro, bool>> BuildFilterExpression(List<FiltroGrid> filtros)
    {
        Expression<Func<Passageiro, bool>> expression = p => true;

        foreach (var filtro in filtros)
        {
            var valor = filtro.Valor.ToLower();

            expression = filtro.Campo.ToLower() switch
            {
                "nome" => CombineAnd(expression, p => p.Nome.ToLower().Contains(valor)),
                "cpf" => CombineAnd(expression, p => p.CPF.Numero.Contains(valor)),
                "email" => CombineAnd(expression, p => p.Email.ToLower().Contains(valor)),
                "telefone" => CombineAnd(expression, p => p.Telefone.Contains(valor)),
                _ => expression
            };
        }

        return expression;
    }

    protected override string MapOrderField(string campoOrdenacao)
    {
        return campoOrdenacao.ToLower() switch
        {
            "nome" => "Nome",
            "cpf" => "CPF",
            "email" => "Email",
            "datanascimento" => "DataNascimento",
            "createdat" => "CreatedAt",
            "updatedat" => "UpdatedAt",
            _ => "Id"
        };
    }

    private Expression<Func<Passageiro, bool>> CombineAnd(
        Expression<Func<Passageiro, bool>> left,
        Expression<Func<Passageiro, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(Passageiro), "p");

        var leftBody = new ParameterRebinder(parameter).Visit(left.Body);
        var rightBody = new ParameterRebinder(parameter).Visit(right.Body);

        var andExpression = Expression.AndAlso(leftBody, rightBody);

        return Expression.Lambda<Func<Passageiro, bool>>(andExpression, parameter);
    }
}
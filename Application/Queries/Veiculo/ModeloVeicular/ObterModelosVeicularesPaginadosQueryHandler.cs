using Application.Common;
using Application.Queries.Veiculo.ModeloVeicular.Models;
using Dominio.Entidades.Veiculos;
using Dominio.Interfaces.Infra.Data;
using Dominio.Enums.Veiculo;
using MediatR;
using System.Linq.Expressions;

namespace Application.Queries.Veiculo.ModeloVeicular;

public class ObterModelosVeicularesPaginadosQueryHandler : IRequestHandler<ObterModelosVeicularesPaginadosQuery, BaseResponse<GridModeloVeicularResult>>
{
    private readonly IGenericRepository<Dominio.Entidades.Veiculos.ModeloVeicular> _modeloVeicularRepository;

    public ObterModelosVeicularesPaginadosQueryHandler(IGenericRepository<Dominio.Entidades.Veiculos.ModeloVeicular> modeloVeicularRepository)
    {
        _modeloVeicularRepository = modeloVeicularRepository;
    }

    public async Task<BaseResponse<GridModeloVeicularResult>> Handle(ObterModelosVeicularesPaginadosQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Monta a expressão de filtro baseada nos filtros recebidos
            Expression<Func<Dominio.Entidades.Veiculos.ModeloVeicular, bool>>? filter = null;
            
            if (request.Filtros?.Any() == true)
            {
                filter = BuildFilterExpression(request.Filtros);
            }

            // Mapeia o campo de ordenação para a propriedade correta
            var orderByProperty = MapOrderField(request.CampoOrdenacao);

            // Busca paginada
            var (items, total) = await _modeloVeicularRepository.GetPaginatedAsync(
                request.PaginaAtual,
                request.TamanhoPagina,
                orderByProperty,
                request.Descendente,
                filter,
                cancellationToken);

            // Converte para DTO com descrições corretas
            var itemsDto = items.Select(m => new ModeloVeicularDto
            {
                Id = m.Id,
                Situacao = m.Situacao,
                SituacaoDescricao = TipoModeloVeiculoHelper.ObterDescricaoSituacao(m.Situacao),
                Descricao = m.Descricao,
                DescricaoModelo = m.Descricao,
                Tipo = m.Tipo,
                TipoDescricao = m.Tipo.ObterDescricao(),
                QuantidadeAssento = m.QuantidadeAssento,
                QuantidadeEixo = m.QuantidadeEixo,
                CapacidadeMaxima = m.CapacidadeMaxima,
                PassageirosEmPe = m.PassageirosEmPe,
                PossuiBanheiro = m.PossuiBanheiro,
                PossuiBanheiroDescricao = TipoModeloVeiculoHelper.ObterDescricaoBoolean(m.PossuiBanheiro),
                PossuiClimatizador = m.PossuiClimatizador,
                PossuiClimatizadorDescricao = TipoModeloVeiculoHelper.ObterDescricaoBoolean(m.PossuiClimatizador),
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt
            });

            var resultado = new GridModeloVeicularResult
            {
                Items = itemsDto,
                Total = total,
                Pagina = request.PaginaAtual,
                TamanhoPagina = request.TamanhoPagina
            };

            return BaseResponse<GridModeloVeicularResult>.Ok(resultado);
        }
        catch (Exception ex)
        {
            return BaseResponse<GridModeloVeicularResult>.Erro($"Erro ao buscar modelos veiculares: {ex.Message}");
        }
    }

    private Expression<Func<Dominio.Entidades.Veiculos.ModeloVeicular, bool>> BuildFilterExpression(List<FiltroGrid> filtros)
    {
        Expression<Func<Dominio.Entidades.Veiculos.ModeloVeicular, bool>> expression = m => true;

        foreach (var filtro in filtros)
        {
            var valor = filtro.Valor.ToLower();

            expression = filtro.Campo.ToLower() switch
            {
                "descricao" => CombineAnd(expression, m => m.Descricao.ToLower().Contains(valor)),
                "descricaomodelo" => CombineAnd(expression, m => m.Descricao.ToLower().Contains(valor)),
                "tipodescricao" => CombineAnd(expression, m => m.Tipo.ObterDescricao().ToLower().Contains(valor)),
                "quantidadeassento" => CombineAnd(expression, m => m.QuantidadeAssento.ToString().Contains(valor)),
                "quantidadeeixo" => CombineAnd(expression, m => m.QuantidadeEixo.ToString().Contains(valor)),
                "capacidademaxima" => CombineAnd(expression, m => m.CapacidadeMaxima.ToString().Contains(valor)),
                "passageirosempe" => CombineAnd(expression, m => m.PassageirosEmPe.ToString().Contains(valor)),
                "situacaodescricao" => valor == "ativo" ? CombineAnd(expression, m => m.Situacao) : valor == "inativo" ? CombineAnd(expression, m => !m.Situacao) : expression,
                "possuibanherio" or "possuibanheodescricao" => valor == "sim" ? CombineAnd(expression, m => m.PossuiBanheiro) : valor == "não" ? CombineAnd(expression, m => !m.PossuiBanheiro) : expression,
                "possuiclimatizador" or "possuiclimatizadordescricao" => valor == "sim" ? CombineAnd(expression, m => m.PossuiClimatizador) : valor == "não" ? CombineAnd(expression, m => !m.PossuiClimatizador) : expression,
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

    private string MapOrderField(string fieldName)
    {
        return fieldName.ToLower() switch
        {
            "descricaomodelo" => "Descricao",
            "tipodescricao" => "Tipo",
            "situacaodescricao" => "Situacao",
            _ => fieldName
        };
    }
} 

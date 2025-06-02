using Application.Common;
using Application.Queries.Veiculo.Models;
using Dominio.Entidades.Veiculos;
using Dominio.Interfaces.Infra.Data;
using MediatR;
using System.Linq.Expressions;

namespace Application.Queries.Veiculo;

public class ObterVeiculosPaginadosQueryHandler : IRequestHandler<ObterVeiculosPaginadosQuery, BaseResponse<GridVeiculoResult>>
{
    private readonly IGenericRepository<Dominio.Entidades.Veiculos.Veiculo> _veiculoRepository;

    public ObterVeiculosPaginadosQueryHandler(IGenericRepository<Dominio.Entidades.Veiculos.Veiculo> veiculoRepository)
    {
        _veiculoRepository = veiculoRepository;
    }

    public async Task<BaseResponse<GridVeiculoResult>> Handle(ObterVeiculosPaginadosQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Monta a expressão de filtro baseada nos filtros recebidos
            Expression<Func<Dominio.Entidades.Veiculos.Veiculo, bool>>? filter = null;
            
            if (request.Filtros?.Any() == true)
            {
                filter = BuildFilterExpression(request.Filtros);
            }

            // Mapeia o campo de ordenação para a propriedade correta
            var orderByProperty = MapOrderField(request.CampoOrdenacao);

            // Busca paginada
            var (items, total) = await _veiculoRepository.GetPaginatedAsync(
                request.PaginaAtual,
                request.TamanhoPagina,
                orderByProperty,
                request.Descendente,
                filter);

            // Converte para DTO
            var itemsDto = items.Select(v => new VeiculoDto
            {
                Id = v.Id,
                Placa = v.Placa ?? "",
                PlacaFormatada = !string.IsNullOrEmpty(v.Placa) ? FormatarPlaca(v.Placa) : "",
                Modelo = v.Modelo ?? "",
                Marca = v.Marca ?? "",
                NumeroChassi = v.NumeroChassi ?? "",
                AnoModelo = v.AnoModelo,
                AnoFabricacao = v.AnoFabricacao,
                Cor = v.Cor ?? "",
                Renavam = v.Renavam ?? "",
                VencimentoLicenciamento = v.VencimentoLicenciamento,
                TipoCombustivel = v.TipoCombustivel,
                TipoCombustivelDescricao = ObterDescricaoEnum(v.TipoCombustivel.ToString()),
                Status = v.Status,
                StatusDescricao = ObterDescricaoEnum(v.Status.ToString()),
                Observacao = v.Observacao ?? "",
                ModeloVeiculoId = v.ModeloVeiculoId,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt,
                Capacidade = 0,
                Ativo = v.Status != Dominio.Enums.Veiculo.StatusVeiculoEnum.Inativo
            });

            var resultado = new GridVeiculoResult
            {
                Items = itemsDto,
                Total = total,
                Pagina = request.PaginaAtual,
                TamanhoPagina = request.TamanhoPagina
            };

            return BaseResponse<GridVeiculoResult>.Ok(resultado);
        }
        catch (Exception ex)
        {
            return BaseResponse<GridVeiculoResult>.Erro($"Erro ao buscar veículos: {ex.Message}");
        }
    }

    private Expression<Func<Dominio.Entidades.Veiculos.Veiculo, bool>> BuildFilterExpression(List<FiltroGrid> filtros)
    {
        Expression<Func<Dominio.Entidades.Veiculos.Veiculo, bool>> expression = v => true;

        foreach (var filtro in filtros)
        {
            var valor = filtro.Valor.ToLower();

            expression = filtro.Campo.ToLower() switch
            {
                "placa" => CombineAnd(expression, v => v.Placa.ToLower().Contains(valor)),
                "placaformatada" => CombineAnd(expression, v => v.Placa.ToLower().Contains(valor)),
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

    private string MapOrderField(string fieldName)
    {
        return fieldName.ToLower() switch
        {
            "placaformatada" => "Placa",
            "statusdescricao" => "Status",
            "tipocombustiveldescricao" => "TipoCombustivel",
            _ => fieldName
        };
    }

    private string FormatarPlaca(string placa)
    {
        if (string.IsNullOrEmpty(placa))
            return string.Empty;

        // Remove qualquer formatação existente
        var placaLimpa = placa.Replace("-", "").Replace(" ", "").ToUpper();

        // Verifica se tem o tamanho correto (7 caracteres)
        if (placaLimpa.Length != 7)
            return placa; // Retorna original se não estiver no formato esperado

        // Formato brasileiro: ABC-1234 ou ABC1D23 (Mercosul)
        if (char.IsLetter(placaLimpa[4])) // Mercosul: ABC1D23
            return $"{placaLimpa.Substring(0, 3)}{placaLimpa.Substring(3, 1)}{placaLimpa.Substring(4, 1)}{placaLimpa.Substring(5, 2)}";
        else // Formato antigo: ABC-1234
            return $"{placaLimpa.Substring(0, 3)}-{placaLimpa.Substring(3, 4)}";
    }

    private string ObterDescricaoEnum(string valor)
    {
        // Implemente a lógica para obter a descrição do enum com base no valor fornecido
        // Este é um exemplo básico e pode ser ajustado conforme necessário
        return valor switch
        {
            "Disponivel" => "Disponível",
            "Indisponivel" => "Indisponível",
            "EmManutencao" => "Em Manutenção",
            "Manutencao" => "Manutenção",
            "EmUso" => "Em Uso",
            _ => valor
        };
    }
} 
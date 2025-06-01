using Application.Common;
using Application.Queries.Veiculo.Models;
using AutoMapper;
using Dominio.Interfaces.Infra.Data.Veiculo;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Veiculo;

public class ListarVeiculosQueryHandler : IRequestHandler<ListarVeiculosQuery, BaseResponse<IEnumerable<VeiculoDto>>>
{
    private readonly IVeiculoRepository _veiculoRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListarVeiculosQueryHandler> _logger;

    public ListarVeiculosQueryHandler(
        IVeiculoRepository veiculoRepository,
        IMapper mapper,
        ILogger<ListarVeiculosQueryHandler> logger)
    {
        _veiculoRepository = veiculoRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<VeiculoDto>>> Handle(ListarVeiculosQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Listando veículos");

            var veiculos = await _veiculoRepository.ObterTodosAsync();
            var veiculosDto = _mapper.Map<IEnumerable<VeiculoDto>>(veiculos);

            _logger.LogInformation("Veículos listados com sucesso. Total: {Total}", veiculosDto.Count());

            return BaseResponse<IEnumerable<VeiculoDto>>.Ok(veiculosDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar veículos");
            return BaseResponse<IEnumerable<VeiculoDto>>.Erro("Erro ao listar veículos", new List<string> { ex.Message });
        }
    }
} 
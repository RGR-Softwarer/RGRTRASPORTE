using Application.Common;
using Application.Queries.Veiculo.Models;
using AutoMapper;
using Dominio.Interfaces.Infra.Data;
using VeiculoEntity = Dominio.Entidades.Veiculos.Veiculo;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Veiculo;

public class ObterVeiculoPorIdQueryHandler : IRequestHandler<ObterVeiculoPorIdQuery, BaseResponse<VeiculoDto>>
{
    private readonly IGenericRepository<VeiculoEntity> _veiculoRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ObterVeiculoPorIdQueryHandler> _logger;

    public ObterVeiculoPorIdQueryHandler(
        IGenericRepository<VeiculoEntity> veiculoRepository,
        IMapper mapper,
        ILogger<ObterVeiculoPorIdQueryHandler> logger)
    {
        _veiculoRepository = veiculoRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponse<VeiculoDto>> Handle(ObterVeiculoPorIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Buscando veículo com ID {VeiculoId}", request.Id);

            var veiculo = await _veiculoRepository.ObterPorIdAsync(request.Id);
            
            if (veiculo == null)
            {
                _logger.LogWarning("Veículo não encontrado com ID {VeiculoId}", request.Id);
                return BaseResponse<VeiculoDto>.Erro("Veículo não encontrado");
            }

            var veiculoDto = _mapper.Map<VeiculoDto>(veiculo);
            
            _logger.LogInformation("Veículo encontrado com sucesso. ID: {VeiculoId}", veiculo.Id);
            
            return BaseResponse<VeiculoDto>.Ok(veiculoDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar veículo");
            return BaseResponse<VeiculoDto>.Erro("Erro ao buscar veículo", new List<string> { ex.Message });
        }
    }
} 

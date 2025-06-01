using Application.Common;
using Application.Queries.Viagem.Models;
using AutoMapper;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Viagem;

public class ObterViagemPorIdQueryHandler : IRequestHandler<ObterViagemPorIdQuery, BaseResponse<ViagemDto>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ObterViagemPorIdQueryHandler> _logger;

    public ObterViagemPorIdQueryHandler(
        IViagemRepository viagemRepository,
        IMapper mapper,
        ILogger<ObterViagemPorIdQueryHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponse<ViagemDto>> Handle(ObterViagemPorIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Buscando viagem com ID {ViagemId}", request.Id);

            var viagem = await _viagemRepository.ObterViagemCompletaPorIdAsync(request.Id);
            
            if (viagem == null)
            {
                _logger.LogWarning("Viagem não encontrada com ID {ViagemId}", request.Id);
                return BaseResponse<ViagemDto>.Erro("Viagem não encontrada");
            }

            var viagemDto = _mapper.Map<ViagemDto>(viagem);
            
            _logger.LogInformation("Viagem encontrada com sucesso. ID: {ViagemId}", viagem.Id);
            
            return BaseResponse<ViagemDto>.Ok(viagemDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar viagem");
            return BaseResponse<ViagemDto>.Erro("Erro ao buscar viagem", new List<string> { ex.Message });
        }
    }
} 
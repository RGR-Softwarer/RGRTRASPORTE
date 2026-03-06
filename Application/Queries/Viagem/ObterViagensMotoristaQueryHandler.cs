using Application.Common;
using Application.Queries.Viagem.Models;
using Application.Queries.Viagem.Mappers;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Viagem;

public class ObterViagensMotoristaQueryHandler : IRequestHandler<ObterViagensMotoristaQuery, BaseResponse<IEnumerable<ViagemDto>>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<ObterViagensMotoristaQueryHandler> _logger;

    public ObterViagensMotoristaQueryHandler(
        IViagemRepository viagemRepository,
        ILogger<ObterViagensMotoristaQueryHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<ViagemDto>>> Handle(ObterViagensMotoristaQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Obtendo viagens do motorista {MotoristaId} para a data {Data}", 
                request.MotoristaId, request.Data);

            var dataInicio = request.Data!.Value.Date;
            var dataFim = dataInicio.AddDays(1).AddTicks(-1);

            var viagens = await _viagemRepository.ObterViagensPorMotoristaEPeriodoAsync(
                request.MotoristaId, dataInicio, dataFim);

            // Mapeamento manual explícito - alinhado com CQRS
            var viagensDto = viagens.ToDto();

            return BaseResponse<IEnumerable<ViagemDto>>.Ok(viagensDto, "Viagens obtidas com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter viagens do motorista {MotoristaId}", request.MotoristaId);
            return BaseResponse<IEnumerable<ViagemDto>>.Erro("Erro ao obter viagens", new List<string> { ex.Message });
        }
    }
}



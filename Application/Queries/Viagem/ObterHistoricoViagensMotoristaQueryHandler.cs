using Application.Common;
using Application.Queries.Viagem.Models;
using Application.Queries.Viagem.Mappers;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Viagem;

public class ObterHistoricoViagensMotoristaQueryHandler : IRequestHandler<ObterHistoricoViagensMotoristaQuery, BaseResponse<IEnumerable<ViagemDto>>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<ObterHistoricoViagensMotoristaQueryHandler> _logger;

    public ObterHistoricoViagensMotoristaQueryHandler(
        IViagemRepository viagemRepository,
        ILogger<ObterHistoricoViagensMotoristaQueryHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<ViagemDto>>> Handle(ObterHistoricoViagensMotoristaQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Obtendo histórico de viagens do motorista {MotoristaId} - DataInicio: {DataInicio}, DataFim: {DataFim}", 
                request.MotoristaId, request.DataInicio, request.DataFim);

            DateTime dataInicio;
            DateTime dataFim;

            if (request.DataInicio.HasValue && request.DataFim.HasValue)
            {
                dataInicio = request.DataInicio.Value.Date;
                dataFim = request.DataFim.Value.Date.AddDays(1).AddTicks(-1);
            }
            else if (request.DataInicio.HasValue)
            {
                dataInicio = request.DataInicio.Value.Date;
                dataFim = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);
            }
            else
            {
                // Por padrão, últimos 90 dias
                dataFim = DateTime.UtcNow.Date.AddDays(-1).AddDays(1).AddTicks(-1);
                dataInicio = dataFim.AddDays(-90).Date;
            }

            var viagens = await _viagemRepository.ObterViagensPorMotoristaEPeriodoAsync(
                request.MotoristaId, dataInicio, dataFim);

            // Ordena por data decrescente (mais recentes primeiro)
            var viagensOrdenadas = viagens.OrderByDescending(v => v.Periodo.Data);

            // Mapeamento manual explícito - alinhado com CQRS
            var viagensDto = viagensOrdenadas.ToDto();

            _logger.LogInformation("Histórico de viagens obtido com sucesso. Total: {Total}", viagensDto.Count());

            return BaseResponse<IEnumerable<ViagemDto>>.Ok(viagensDto, "Histórico de viagens obtido com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter histórico de viagens do motorista {MotoristaId}", request.MotoristaId);
            return BaseResponse<IEnumerable<ViagemDto>>.Erro("Erro ao obter histórico de viagens", new List<string> { ex.Message });
        }
    }
}


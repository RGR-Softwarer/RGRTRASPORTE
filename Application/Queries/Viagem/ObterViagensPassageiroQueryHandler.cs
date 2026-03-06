using Application.Common;
using Application.Queries.Viagem.Models;
using Application.Queries.Viagem.Mappers;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Viagem;

public class ObterViagensPassageiroQueryHandler : IRequestHandler<ObterViagensPassageiroQuery, BaseResponse<IEnumerable<ViagemDto>>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<ObterViagensPassageiroQueryHandler> _logger;

    public ObterViagensPassageiroQueryHandler(
        IViagemRepository viagemRepository,
        ILogger<ObterViagensPassageiroQueryHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<ViagemDto>>> Handle(ObterViagensPassageiroQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Obtendo viagens do passageiro {PassageiroId}", request.PassageiroId);

            var dataInicio = request.DataInicio?.Date ?? DateTime.UtcNow.Date;
            var dataFim = request.DataFim?.Date ?? dataInicio.AddDays(30);
            
            // Garantir UTC
            dataInicio = DateTime.SpecifyKind(dataInicio, DateTimeKind.Utc);
            dataFim = DateTime.SpecifyKind(dataFim, DateTimeKind.Utc);

            _logger.LogInformation("Buscando viagens do passageiro {PassageiroId} - DataInicio: {DataInicio}, DataFim: {DataFim}", 
                request.PassageiroId, dataInicio, dataFim);

            var viagens = await _viagemRepository.ObterViagensPassageiroAsync(request.PassageiroId, dataInicio, dataFim);
            
            _logger.LogInformation("Viagens encontradas para passageiro {PassageiroId}: {Count}", request.PassageiroId, viagens.Count());

            // Mapeamento manual explícito - alinhado com CQRS
            var viagensDto = viagens.ToDto();

            return BaseResponse<IEnumerable<ViagemDto>>.Ok(viagensDto, "Viagens obtidas com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter viagens do passageiro {PassageiroId}", request.PassageiroId);
            return BaseResponse<IEnumerable<ViagemDto>>.Erro("Erro ao obter viagens", new List<string> { ex.Message });
        }
    }
}


using Application.Common;
using Application.Queries.Viagem.Models;
using Application.Queries.Viagem.Mappers;
using Dominio.Enums.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Viagem;

public class ObterViagensPendentesConfirmacaoQueryHandler : IRequestHandler<ObterViagensPendentesConfirmacaoQuery, BaseResponse<IEnumerable<ViagemDto>>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<ObterViagensPendentesConfirmacaoQueryHandler> _logger;

    public ObterViagensPendentesConfirmacaoQueryHandler(
        IViagemRepository viagemRepository,
        ILogger<ObterViagensPendentesConfirmacaoQueryHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<ViagemDto>>> Handle(ObterViagensPendentesConfirmacaoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Obtendo viagens pendentes de confirmação para passageiro {PassageiroId}", request.PassageiroId);

            var hoje = DateTime.UtcNow.Date;
            var dataLimite = hoje.AddDays(7); // Próximos 7 dias

            var viagens = await _viagemRepository.ObterViagensPassageiroAsync(request.PassageiroId, hoje, dataLimite);

            var viagensPendentes = viagens
                .Where(v => v.Situacao == SituacaoViagemEnum.Agendada)
                .Where(v => v.Passageiros.Any(p => 
                    p.PassageiroId == request.PassageiroId && 
                    p.StatusConfirmacao == StatusConfirmacaoEnum.AguardandoConfirmacao))
                .ToList();

            // Mapeamento manual explícito - alinhado com CQRS
            var viagensDto = viagensPendentes.ToDto();

            return BaseResponse<IEnumerable<ViagemDto>>.Ok(viagensDto, "Viagens pendentes obtidas com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter viagens pendentes para passageiro {PassageiroId}", request.PassageiroId);
            return BaseResponse<IEnumerable<ViagemDto>>.Erro("Erro ao obter viagens pendentes", new List<string> { ex.Message });
        }
    }
}


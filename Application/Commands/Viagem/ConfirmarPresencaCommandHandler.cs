using Application.Common;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem;

public class ConfirmarPresencaCommandHandler : IRequestHandler<ConfirmarPresencaCommand, BaseResponse<bool>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<ConfirmarPresencaCommandHandler> _logger;

    public ConfirmarPresencaCommandHandler(
        IViagemRepository viagemRepository,
        ILogger<ConfirmarPresencaCommandHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(ConfirmarPresencaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Confirmando presença do passageiro {PassageiroId} na viagem {ViagemId}", 
                request.PassageiroId, request.ViagemId);

            var viagem = await _viagemRepository.ObterPorIdAsync(request.ViagemId, cancellationToken);
            if (viagem == null)
                return BaseResponse<bool>.Erro("Viagem não encontrada");

            var viagemPassageiro = viagem.ObterPassageiro(request.PassageiroId);
            if (viagemPassageiro == null)
                return BaseResponse<bool>.Erro("Passageiro não encontrado na viagem");

            viagemPassageiro.ConfirmarPresenca();
            await _viagemRepository.AtualizarAsync(viagem, cancellationToken);

            _logger.LogInformation("Presença confirmada com sucesso para passageiro {PassageiroId} na viagem {ViagemId}", 
                request.PassageiroId, request.ViagemId);

            return BaseResponse<bool>.Ok(true, "Presença confirmada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao confirmar presença do passageiro {PassageiroId} na viagem {ViagemId}", 
                request.PassageiroId, request.ViagemId);
            return BaseResponse<bool>.Erro("Erro ao confirmar presença", new List<string> { ex.Message });
        }
    }
}



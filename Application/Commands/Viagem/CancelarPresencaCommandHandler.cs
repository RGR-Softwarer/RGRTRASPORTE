using Application.Common;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem;

public class CancelarPresencaCommandHandler : IRequestHandler<CancelarPresencaCommand, BaseResponse<bool>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<CancelarPresencaCommandHandler> _logger;

    public CancelarPresencaCommandHandler(
        IViagemRepository viagemRepository,
        ILogger<CancelarPresencaCommandHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(CancelarPresencaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Cancelando presença do passageiro {PassageiroId} na viagem {ViagemId}", 
                request.PassageiroId, request.ViagemId);

            var viagem = await _viagemRepository.ObterPorIdAsync(request.ViagemId, cancellationToken);
            if (viagem == null)
                return BaseResponse<bool>.Erro("Viagem não encontrada");

            var viagemPassageiro = viagem.ObterPassageiro(request.PassageiroId);
            if (viagemPassageiro == null)
                return BaseResponse<bool>.Erro("Passageiro não encontrado na viagem");

            viagemPassageiro.CancelarPresenca(request.Motivo);
            await _viagemRepository.AtualizarAsync(viagem, cancellationToken);

            _logger.LogInformation("Presença cancelada com sucesso para passageiro {PassageiroId} na viagem {ViagemId}", 
                request.PassageiroId, request.ViagemId);

            return BaseResponse<bool>.Ok(true, "Presença cancelada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao cancelar presença do passageiro {PassageiroId} na viagem {ViagemId}", 
                request.PassageiroId, request.ViagemId);
            return BaseResponse<bool>.Erro("Erro ao cancelar presença", new List<string> { ex.Message });
        }
    }
}



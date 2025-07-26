using Application.Common;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem;

public class CancelarViagemCommandHandler : IRequestHandler<CancelarViagemCommand, BaseResponse<bool>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<CancelarViagemCommandHandler> _logger;

    public CancelarViagemCommandHandler(
        IViagemRepository viagemRepository,
        ILogger<CancelarViagemCommandHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(CancelarViagemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando cancelamento da viagem {ViagemId}", request.Id);

            var viagem = await _viagemRepository.ObterPorIdAsync(request.Id);
            if (viagem == null)
                return BaseResponse<bool>.Erro("Viagem não encontrada");

            viagem.CancelarViagem(request.Motivo);

            await _viagemRepository.AtualizarAsync(viagem);

            _logger.LogInformation("Viagem {ViagemId} cancelada com sucesso", request.Id);

            return BaseResponse<bool>.Ok(true, "Viagem cancelada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao cancelar viagem {ViagemId}", request.Id);
            return BaseResponse<bool>.Erro("Erro ao cancelar viagem", new List<string> { ex.Message });
        }
    }
} 
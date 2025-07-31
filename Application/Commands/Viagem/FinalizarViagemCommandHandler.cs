using Application.Common;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem;

public class FinalizarViagemCommandHandler : IRequestHandler<FinalizarViagemCommand, BaseResponse<bool>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<FinalizarViagemCommandHandler> _logger;

    public FinalizarViagemCommandHandler(
        IViagemRepository viagemRepository,
        ILogger<FinalizarViagemCommandHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(FinalizarViagemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Finalizando a viagem {ViagemId}", request.Id);

            var viagem = await _viagemRepository.ObterPorIdAsync(request.Id);
            if (viagem == null)
                return BaseResponse<bool>.Erro("Viagem não encontrada");

            viagem.FinalizarViagem();

            await _viagemRepository.AtualizarAsync(viagem);

            _logger.LogInformation("Viagem {ViagemId} finalizada com sucesso", request.Id);

            return BaseResponse<bool>.Ok(true, "Viagem finalizada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao finalizar viagem {ViagemId}", request.Id);
            return BaseResponse<bool>.Erro("Erro ao finalizar viagem", new List<string> { ex.Message });
        }
    }
} 

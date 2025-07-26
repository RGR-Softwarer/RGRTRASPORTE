using Application.Common;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem;

public class RemoverPassageiroViagemCommandHandler : IRequestHandler<RemoverPassageiroViagemCommand, BaseResponse<bool>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<RemoverPassageiroViagemCommandHandler> _logger;

    public RemoverPassageiroViagemCommandHandler(
        IViagemRepository viagemRepository,
        ILogger<RemoverPassageiroViagemCommandHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(RemoverPassageiroViagemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Removendo passageiro {PassageiroId} da viagem {ViagemId}", request.PassageiroId, request.ViagemId);

            var viagem = await _viagemRepository.ObterPorIdAsync(request.ViagemId);
            if (viagem == null)
                return BaseResponse<bool>.Erro("Viagem não encontrada");

            viagem.RemoverPassageiro(request.PassageiroId);
            await _viagemRepository.AtualizarAsync(viagem);

            _logger.LogInformation("Passageiro {PassageiroId} removido com sucesso da viagem {ViagemId}", request.PassageiroId, request.ViagemId);

            return BaseResponse<bool>.Ok(true, "Passageiro removido da viagem com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover passageiro {PassageiroId} da viagem {ViagemId}", request.PassageiroId, request.ViagemId);
            return BaseResponse<bool>.Erro("Erro ao remover passageiro da viagem", new List<string> { ex.Message });
        }
    }
} 
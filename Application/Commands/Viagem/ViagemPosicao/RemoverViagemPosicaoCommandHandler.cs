using Application.Common;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem.ViagemPosicao;

public class RemoverViagemPosicaoCommandHandler : IRequestHandler<RemoverViagemPosicaoCommand, BaseResponse<bool>>
{
    private readonly IViagemPosicaoRepository _viagemPosicaoRepository;
    private readonly ILogger<RemoverViagemPosicaoCommandHandler> _logger;

    public RemoverViagemPosicaoCommandHandler(
        IViagemPosicaoRepository viagemPosicaoRepository,
        ILogger<RemoverViagemPosicaoCommandHandler> logger)
    {
        _viagemPosicaoRepository = viagemPosicaoRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(RemoverViagemPosicaoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Removendo posição {PosicaoId}", request.Id);

            var viagemPosicao = await _viagemPosicaoRepository.ObterPorIdAsync(request.Id);
            if (viagemPosicao == null)
                return BaseResponse<bool>.Erro("Posição não encontrada");

            await _viagemPosicaoRepository.RemoverAsync(viagemPosicao);

            _logger.LogInformation("Posição {PosicaoId} removida com sucesso", request.Id);

            return BaseResponse<bool>.Ok(true, "Posição removida com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover posição {PosicaoId}", request.Id);
            return BaseResponse<bool>.Erro("Erro ao remover posição", new List<string> { ex.Message });
        }
    }
} 
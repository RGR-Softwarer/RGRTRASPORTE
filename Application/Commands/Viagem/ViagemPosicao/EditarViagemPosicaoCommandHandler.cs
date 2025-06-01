using Application.Common;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem.ViagemPosicao;

public class EditarViagemPosicaoCommandHandler : IRequestHandler<EditarViagemPosicaoCommand, BaseResponse<bool>>
{
    private readonly IViagemPosicaoRepository _viagemPosicaoRepository;
    private readonly ILogger<EditarViagemPosicaoCommandHandler> _logger;

    public EditarViagemPosicaoCommandHandler(
        IViagemPosicaoRepository viagemPosicaoRepository,
        ILogger<EditarViagemPosicaoCommandHandler> logger)
    {
        _viagemPosicaoRepository = viagemPosicaoRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(EditarViagemPosicaoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Editando posição {PosicaoId}", request.Id);

            var viagemPosicao = await _viagemPosicaoRepository.ObterPorIdAsync(request.Id);
            if (viagemPosicao == null)
                return BaseResponse<bool>.Erro("Posição não encontrada");

            viagemPosicao.Atualizar(
                viagemPosicao.ViagemId,
                request.DataPosicao,
                request.Latitude.ToString(),
                request.Longitude.ToString());

            await _viagemPosicaoRepository.AtualizarAsync(viagemPosicao);

            _logger.LogInformation("Posição {PosicaoId} atualizada com sucesso", request.Id);

            return BaseResponse<bool>.Ok(true, "Posição atualizada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao editar posição {PosicaoId}", request.Id);
            return BaseResponse<bool>.Erro("Erro ao editar posição", new List<string> { ex.Message });
        }
    }
} 
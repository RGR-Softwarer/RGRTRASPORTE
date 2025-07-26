using Application.Common;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem;

public class EditarViagemCommandHandler : IRequestHandler<EditarViagemCommand, BaseResponse<bool>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<EditarViagemCommandHandler> _logger;

    public EditarViagemCommandHandler(
        IViagemRepository viagemRepository,
        ILogger<EditarViagemCommandHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(EditarViagemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando edição da viagem {ViagemId}", request.Id);

            var viagem = await _viagemRepository.ObterPorIdAsync(request.Id);
            if (viagem == null)
                return BaseResponse<bool>.Erro("Viagem não encontrada");

            viagem.Atualizar(
                request.DataViagem,
                request.HorarioSaida,
                request.HorarioChegada,
                request.VeiculoId,
                request.LocalidadeOrigemId,
                request.LocalidadeDestinoId,
                request.ValorPassagem,
                request.QuantidadeVagas,
                request.Ativo,
                request.GatilhoViagemId);

            await _viagemRepository.AtualizarAsync(viagem);

            _logger.LogInformation("Viagem {ViagemId} atualizada com sucesso", request.Id);

            return BaseResponse<bool>.Ok(true, "Viagem atualizada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao editar viagem {ViagemId}", request.Id);
            return BaseResponse<bool>.Erro("Erro ao editar viagem", new List<string> { ex.Message });
        }
    }
} 
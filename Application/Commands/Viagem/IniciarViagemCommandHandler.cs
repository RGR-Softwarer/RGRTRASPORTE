using Application.Common;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem;

public class IniciarViagemCommandHandler : IRequestHandler<IniciarViagemCommand, BaseResponse<bool>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<IniciarViagemCommandHandler> _logger;

    public IniciarViagemCommandHandler(
        IViagemRepository viagemRepository,
        ILogger<IniciarViagemCommandHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(IniciarViagemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando a viagem {ViagemId}", request.Id);

            var viagem = await _viagemRepository.ObterPorIdAsync(request.Id);
            if (viagem == null)
                return BaseResponse<bool>.Erro("Viagem não encontrada");

            viagem.IniciarViagem();

            await _viagemRepository.AtualizarAsync(viagem);

            _logger.LogInformation("Viagem {ViagemId} iniciada com sucesso", request.Id);

            return BaseResponse<bool>.Ok(true, "Viagem iniciada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao iniciar viagem {ViagemId}", request.Id);
            return BaseResponse<bool>.Erro("Erro ao iniciar viagem", new List<string> { ex.Message });
        }
    }
} 

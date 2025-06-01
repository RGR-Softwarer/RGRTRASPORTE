using Application.Common;
using Dominio.Interfaces.Infra.Data.Passageiros;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Passageiro;

public class RemoverPassageiroCommandHandler : IRequestHandler<RemoverPassageiroCommand, BaseResponse<bool>>
{
    private readonly IPassageiroRepository _passageiroRepository;
    private readonly ILogger<RemoverPassageiroCommandHandler> _logger;

    public RemoverPassageiroCommandHandler(
        IPassageiroRepository passageiroRepository,
        ILogger<RemoverPassageiroCommandHandler> logger)
    {
        _passageiroRepository = passageiroRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(RemoverPassageiroCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando remoção do passageiro {PassageiroId}", request.Id);

            var passageiro = await _passageiroRepository.ObterPorIdAsync(request.Id);
            if (passageiro == null)
                return BaseResponse<bool>.Erro("Passageiro não encontrado");

            await _passageiroRepository.RemoverAsync(passageiro);

            _logger.LogInformation("Passageiro {PassageiroId} removido com sucesso", request.Id);

            return BaseResponse<bool>.Ok(true, "Passageiro removido com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover passageiro {PassageiroId}", request.Id);
            return BaseResponse<bool>.Erro("Erro ao remover passageiro", new List<string> { ex.Message });
        }
    }
} 
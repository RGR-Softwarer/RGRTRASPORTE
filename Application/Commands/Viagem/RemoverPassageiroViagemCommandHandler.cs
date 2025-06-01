using Application.Common;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem;

public class RemoverPassageiroViagemCommandHandler : IRequestHandler<RemoverPassageiroViagemCommand, BaseResponse<bool>>
{
    private readonly IViagemPassageiroRepository _viagemPassageiroRepository;
    private readonly ILogger<RemoverPassageiroViagemCommandHandler> _logger;

    public RemoverPassageiroViagemCommandHandler(
        IViagemPassageiroRepository viagemPassageiroRepository,
        ILogger<RemoverPassageiroViagemCommandHandler> logger)
    {
        _viagemPassageiroRepository = viagemPassageiroRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(RemoverPassageiroViagemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Removendo passageiro {PassageiroId} da viagem {ViagemId}", request.PassageiroId, request.ViagemId);

            // Busca por passageiros da viagem e filtra pelo passageiro específico
            var passageirosDaViagem = await _viagemPassageiroRepository.ObterPassageirosPorViagemAsync(request.ViagemId);
            var viagemPassageiro = passageirosDaViagem.FirstOrDefault(vp => vp.PassageiroId == request.PassageiroId);
            
            if (viagemPassageiro == null)
                return BaseResponse<bool>.Erro("Passageiro não encontrado na viagem");

            await _viagemPassageiroRepository.RemoverAsync(viagemPassageiro);

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
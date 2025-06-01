using Application.Common;
using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem;

public class AdicionarPassageiroViagemCommandHandler : IRequestHandler<AdicionarPassageiroViagemCommand, BaseResponse<bool>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly IViagemPassageiroRepository _viagemPassageiroRepository;
    private readonly ILogger<AdicionarPassageiroViagemCommandHandler> _logger;

    public AdicionarPassageiroViagemCommandHandler(
        IViagemRepository viagemRepository,
        IViagemPassageiroRepository viagemPassageiroRepository,
        ILogger<AdicionarPassageiroViagemCommandHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _viagemPassageiroRepository = viagemPassageiroRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(AdicionarPassageiroViagemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Adicionando passageiro {PassageiroId} à viagem {ViagemId}", request.PassageiroId, request.ViagemId);

            var viagem = await _viagemRepository.ObterPorIdAsync(request.ViagemId);
            if (viagem == null)
                return BaseResponse<bool>.Erro("Viagem não encontrada");

            viagem.ReservarVaga();

            var viagemPassageiro = new ViagemPassageiro(request.ViagemId, request.PassageiroId);
            await _viagemPassageiroRepository.AdicionarAsync(viagemPassageiro);
            await _viagemRepository.AtualizarAsync(viagem);

            _logger.LogInformation("Passageiro {PassageiroId} adicionado à viagem {ViagemId} com sucesso", request.PassageiroId, request.ViagemId);

            return BaseResponse<bool>.Ok(true, "Passageiro adicionado à viagem com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar passageiro {PassageiroId} à viagem {ViagemId}", request.PassageiroId, request.ViagemId);
            return BaseResponse<bool>.Erro("Erro ao adicionar passageiro à viagem", new List<string> { ex.Message });
        }
    }
} 
using Application.Common;
using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using Dominio.Interfaces.Infra.Data.Passageiros;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem;

public class AdicionarPassageiroViagemCommandHandler : IRequestHandler<AdicionarPassageiroViagemCommand, BaseResponse<bool>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly IPassageiroRepository _passageiroRepository;
    private readonly ILogger<AdicionarPassageiroViagemCommandHandler> _logger;

    public AdicionarPassageiroViagemCommandHandler(
        IViagemRepository viagemRepository,
        IPassageiroRepository passageiroRepository,
        ILogger<AdicionarPassageiroViagemCommandHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _passageiroRepository = passageiroRepository;
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

            var passageiro = await _passageiroRepository.ObterPorIdAsync(request.PassageiroId);
            if (passageiro == null)
                return BaseResponse<bool>.Erro("Passageiro não encontrado");

            viagem.AdicionarPassageiro(passageiro);
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
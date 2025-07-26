using Application.Common;
using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem.ViagemPosicao;

public class AdicionarViagemPosicaoCommandHandler : IRequestHandler<AdicionarViagemPosicaoCommand, BaseResponse<long>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<AdicionarViagemPosicaoCommandHandler> _logger;

    public AdicionarViagemPosicaoCommandHandler(
        IViagemRepository viagemRepository,
        ILogger<AdicionarViagemPosicaoCommandHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<long>> Handle(AdicionarViagemPosicaoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Adicionando posição para a viagem {ViagemId}", request.ViagemId);

            var viagem = await _viagemRepository.ObterPorIdAsync(request.ViagemId);
            if (viagem == null)
                return BaseResponse<long>.Erro("Viagem não encontrada");

            viagem.AdicionarPosicao(request.Latitude, request.Longitude, request.DataPosicao);
            await _viagemRepository.AtualizarAsync(viagem);

            var posicao = viagem.Posicoes.Last();
            _logger.LogInformation("Posição adicionada com sucesso para a viagem {ViagemId}", request.ViagemId);

            return BaseResponse<long>.Ok(posicao.Id, "Posição adicionada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar posição para a viagem {ViagemId}", request.ViagemId);
            return BaseResponse<long>.Erro("Erro ao adicionar posição", new List<string> { ex.Message });
        }
    }
} 
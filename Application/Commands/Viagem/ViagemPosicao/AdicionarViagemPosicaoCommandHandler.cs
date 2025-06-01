using Application.Common;
using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem.ViagemPosicao;

public class AdicionarViagemPosicaoCommandHandler : IRequestHandler<AdicionarViagemPosicaoCommand, BaseResponse<long>>
{
    private readonly IViagemPosicaoRepository _viagemPosicaoRepository;
    private readonly ILogger<AdicionarViagemPosicaoCommandHandler> _logger;

    public AdicionarViagemPosicaoCommandHandler(
        IViagemPosicaoRepository viagemPosicaoRepository,
        ILogger<AdicionarViagemPosicaoCommandHandler> logger)
    {
        _viagemPosicaoRepository = viagemPosicaoRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<long>> Handle(AdicionarViagemPosicaoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Adicionando posição para a viagem {ViagemId}", request.ViagemId);

            var viagemPosicao = new Dominio.Entidades.Viagens.ViagemPosicao(
                request.ViagemId,
                request.DataPosicao,
                request.Latitude.ToString(),
                request.Longitude.ToString());

            await _viagemPosicaoRepository.AdicionarAsync(viagemPosicao);

            _logger.LogInformation("Posição adicionada com sucesso para a viagem {ViagemId}", request.ViagemId);

            return BaseResponse<long>.Ok(viagemPosicao.Id, "Posição adicionada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar posição para a viagem {ViagemId}", request.ViagemId);
            return BaseResponse<long>.Erro("Erro ao adicionar posição", new List<string> { ex.Message });
        }
    }
} 
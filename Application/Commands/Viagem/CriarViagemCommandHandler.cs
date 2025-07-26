using Application.Common;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem;

public class CriarViagemCommandHandler : IRequestHandler<CriarViagemCommand, BaseResponse<long>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<CriarViagemCommandHandler> _logger;

    public CriarViagemCommandHandler(
        IViagemRepository viagemRepository,
        ILogger<CriarViagemCommandHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<long>> Handle(CriarViagemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando criação de viagem para data {DataViagem}", request.DataViagem);

            var viagem = new Dominio.Entidades.Viagens.Viagem(
                request.DataViagem,
                request.HorarioSaida,
                request.HorarioChegada,
                request.VeiculoId,
                request.MotoristaId,
                request.LocalidadeOrigemId,
                request.LocalidadeDestinoId,
                request.ValorPassagem,
                request.QuantidadeVagas,
                request.Distancia,
                request.DescricaoViagem,
                request.PolilinhaRota,
                request.Ativo,
                request.GatilhoViagemId);

            await _viagemRepository.AdicionarAsync(viagem);

            _logger.LogInformation("Viagem criada com sucesso. ID: {ViagemId}", viagem.Id);

            return BaseResponse<long>.Ok(viagem.Id, "Viagem criada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar viagem");
            return BaseResponse<long>.Erro("Erro ao criar viagem", new List<string> { ex.Message });
        }
    }
} 
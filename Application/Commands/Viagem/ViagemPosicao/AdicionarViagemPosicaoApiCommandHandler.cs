using Application.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem.ViagemPosicao;

public class AdicionarViagemPosicaoApiCommandHandler : IRequestHandler<AdicionarViagemPosicaoApiCommand, BaseResponse<long>>
{
    private readonly IMediator _mediator;
    private readonly ILogger<AdicionarViagemPosicaoApiCommandHandler> _logger;

    public AdicionarViagemPosicaoApiCommandHandler(
        IMediator mediator,
        ILogger<AdicionarViagemPosicaoApiCommandHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<BaseResponse<long>> Handle(AdicionarViagemPosicaoApiCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("API Command - Adicionando posição para a viagem {ViagemId}", request.ViagemId);

        // Usa GetDataPosicao() que aceita tanto "dataPosicao" quanto "dataHora"
        var dataPosicao = request.GetDataPosicao();
        
        // Delega para o command handler existente
        var command = new AdicionarViagemPosicaoCommand(request.ViagemId, request.Latitude, request.Longitude, dataPosicao);
        return await _mediator.Send(command, cancellationToken);
    }
} 

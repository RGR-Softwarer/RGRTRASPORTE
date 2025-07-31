using Application.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Viagem;

public class AdicionarPassageiroViagemApiCommandHandler : IRequestHandler<AdicionarPassageiroViagemApiCommand, BaseResponse<bool>>
{
    private readonly IMediator _mediator;
    private readonly ILogger<AdicionarPassageiroViagemApiCommandHandler> _logger;

    public AdicionarPassageiroViagemApiCommandHandler(
        IMediator mediator,
        ILogger<AdicionarPassageiroViagemApiCommandHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(AdicionarPassageiroViagemApiCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("API Command - Adicionando passageiro {PassageiroId} à viagem {ViagemId}", request.PassageiroId, request.ViagemId);

        // Delega para o command handler existente
        var command = new AdicionarPassageiroViagemCommand(request.ViagemId, request.PassageiroId);
        return await _mediator.Send(command, cancellationToken);
    }
} 

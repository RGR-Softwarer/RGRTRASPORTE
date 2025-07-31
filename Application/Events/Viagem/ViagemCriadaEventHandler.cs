using Dominio.Events.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Viagem;

public class ViagemCriadaEventHandler : INotificationHandler<ViagemCriadaEvent>
{
    private readonly ILogger<ViagemCriadaEventHandler> _logger;

    public ViagemCriadaEventHandler(ILogger<ViagemCriadaEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ViagemCriadaEvent notification, CancellationToken cancellationToken)
    {        

        // Aqui voc� pode adicionar l�gica adicional para processar o evento
        // Por exemplo: enviar notifica��es, atualizar cache, etc.

        return Task.CompletedTask;
    }
}

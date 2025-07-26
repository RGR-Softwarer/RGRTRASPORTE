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

        // Aqui você pode adicionar lógica adicional para processar o evento
        // Por exemplo: enviar notificações, atualizar cache, etc.

        return Task.CompletedTask;
    }
}

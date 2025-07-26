using Dominio.Events;
using Dominio.Events.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Viagem;

public class ViagemFinalizadaEventHandler : INotificationHandler<ViagemFinalizadaEvent>
{
    private readonly ILogger<ViagemFinalizadaEventHandler> _logger;

    public ViagemFinalizadaEventHandler(ILogger<ViagemFinalizadaEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ViagemFinalizadaEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Viagem {ViagemId} finalizada em {DataFinalizacao}", 
            notification.ViagemId, notification.DataFinalizacao);

        // Aqui você pode adicionar lógica adicional para processar o evento
        // Por exemplo: enviar notificações, atualizar cache, etc.

        return Task.CompletedTask;
    }
} 
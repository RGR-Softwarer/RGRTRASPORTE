using Dominio.Events.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Viagem;

public class ViagemRemovidaEventHandler : INotificationHandler<ViagemRemovidaEvent>
{
    private readonly ILogger<ViagemRemovidaEventHandler> _logger;

    public ViagemRemovidaEventHandler(ILogger<ViagemRemovidaEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ViagemRemovidaEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Viagem {ViagemId} removida em {DataRemocao}", 
            notification.ViagemId,  DateTime.UtcNow);

        // Aqui você pode adicionar lógica adicional para processar o evento
        // Por exemplo: enviar notificações, atualizar cache, etc.

        return Task.CompletedTask;
    }
} 
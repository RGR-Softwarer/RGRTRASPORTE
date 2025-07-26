using Dominio.Events;
using Dominio.Events.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Viagem;

public class ViagemIniciadaEventHandler : INotificationHandler<ViagemIniciadaEvent>
{
    private readonly ILogger<ViagemIniciadaEventHandler> _logger;

    public ViagemIniciadaEventHandler(ILogger<ViagemIniciadaEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ViagemIniciadaEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Viagem {ViagemId} iniciada em {DataInicio}", 
            notification.ViagemId, notification.DataInicio);

        // Aqui você pode adicionar lógica adicional para processar o evento
        // Por exemplo: enviar notificações, atualizar cache, etc.

        return Task.CompletedTask;
    }
} 
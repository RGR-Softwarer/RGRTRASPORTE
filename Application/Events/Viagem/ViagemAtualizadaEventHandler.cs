using Dominio.Events.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Viagem
{
    public class ViagemAtualizadaEventHandler : INotificationHandler<ViagemAtualizadaEvent>
    {
        private readonly ILogger<ViagemAtualizadaEventHandler> _logger;

        public ViagemAtualizadaEventHandler(ILogger<ViagemAtualizadaEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ViagemAtualizadaEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Viagem {ViagemId} foi atualizada", notification.ViagemId);
            
            // TODO: Implementar processamento específico se necessário
            
            return Task.CompletedTask;
        }
    }
} 

using Dominio.Events.Veiculos;
using Dominio.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.ModeloVeicular
{
    public class ModeloVeicularCriadoEventHandler : INotificationHandler<ModeloVeicularCriadoEvent>
    {
        private readonly ILogger<ModeloVeicularCriadoEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public ModeloVeicularCriadoEventHandler(
            ILogger<ModeloVeicularCriadoEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(ModeloVeicularCriadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Modelo veicular criado: {ModeloVeicularId} - {Descricao} - {Tipo}", 
                notification.ModeloVeicularId, notification.Descricao, notification.Tipo);

            await _auditoriaService.RegistrarEventoAsync(
                "ModeloVeicularCriado",
                notification.ModeloVeicularId,
                $"Modelo {notification.Descricao} do tipo {notification.Tipo} foi criado",
                "Sistema",
                "127.0.0.1",
                cancellationToken);
        }
    }

    public class ModeloVeicularAtualizadoEventHandler : INotificationHandler<ModeloVeicularAtualizadoEvent>
    {
        private readonly ILogger<ModeloVeicularAtualizadoEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public ModeloVeicularAtualizadoEventHandler(
            ILogger<ModeloVeicularAtualizadoEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(ModeloVeicularAtualizadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Modelo veicular atualizado: {ModeloVeicularId} - {Descricao}", 
                notification.ModeloVeicularId, notification.Descricao);

            await _auditoriaService.RegistrarEventoAsync(
                "ModeloVeicularAtualizado",
                notification.ModeloVeicularId,
                $"Modelo {notification.Descricao} foi atualizado",
                "Sistema",
                "127.0.0.1",
                cancellationToken);
        }
    }

    public class ModeloVeicularAtivadoEventHandler : INotificationHandler<ModeloVeicularAtivadoEvent>
    {
        private readonly ILogger<ModeloVeicularAtivadoEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public ModeloVeicularAtivadoEventHandler(
            ILogger<ModeloVeicularAtivadoEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(ModeloVeicularAtivadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Modelo veicular ativado: {ModeloVeicularId} - {Descricao}", 
                notification.ModeloVeicularId, notification.Descricao);

            await _auditoriaService.RegistrarEventoAsync(
                "ModeloVeicularAtivado",
                notification.ModeloVeicularId,
                $"Modelo {notification.Descricao} foi ativado",
                "Sistema",
                "127.0.0.1",
                cancellationToken);
        }
    }

    public class ModeloVeicularInativadoEventHandler : INotificationHandler<ModeloVeicularInativadoEvent>
    {
        private readonly ILogger<ModeloVeicularInativadoEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public ModeloVeicularInativadoEventHandler(
            ILogger<ModeloVeicularInativadoEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(ModeloVeicularInativadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Modelo veicular inativado: {ModeloVeicularId} - {Descricao}", 
                notification.ModeloVeicularId, notification.Descricao);

            await _auditoriaService.RegistrarEventoAsync(
                "ModeloVeicularInativado",
                notification.ModeloVeicularId,
                $"Modelo {notification.Descricao} foi inativado",
                "Sistema",
                "127.0.0.1",
                cancellationToken);
        }
    }
}

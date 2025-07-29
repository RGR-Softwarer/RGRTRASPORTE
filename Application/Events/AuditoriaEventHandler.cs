using Dominio.Events.Base;
using Dominio.Services;
using Infra.CrossCutting.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Events
{
    public class AuditoriaEventHandler : INotificationHandler<DomainEvent>
    {
        private readonly IAuditoriaService _auditoriaService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AuditoriaEventHandler> _logger;

        public AuditoriaEventHandler(
            IAuditoriaService auditoriaService,
            ICurrentUserService currentUserService,
            ILogger<AuditoriaEventHandler> logger)
        {
            _auditoriaService = auditoriaService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task Handle(DomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var nomeEvento = notification.GetType().Name;
                var entidadeId = ObterEntidadeId(notification);
                var dadosEvento = JsonSerializer.Serialize(notification, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await _auditoriaService.RegistrarEventoAsync(
                    nomeEvento,
                    entidadeId,
                    dadosEvento,
                    _currentUserService.UserName ?? "Sistema",
                    _currentUserService.IpAddress ?? "Unknown"
                );

                _logger.LogInformation("Evento {NomeEvento} auditado para entidade {EntidadeId} pelo usuário {Usuario}", 
                    nomeEvento, entidadeId, _currentUserService.UserName ?? "Sistema");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao auditar evento {TipoEvento}", 
                    notification.GetType().Name);
            }
        }

        private long ObterEntidadeId(DomainEvent evento)
        {
            // Tenta extrair o ID da entidade usando reflection
            var propriedades = evento.GetType().GetProperties();
            
            // Procura por propriedades que terminam com "Id"
            var propriedadeId = propriedades.FirstOrDefault(p => 
                p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase) && 
                p.PropertyType == typeof(long));

            if (propriedadeId != null)
            {
                var valor = propriedadeId.GetValue(evento);
                if (valor is long id)
                    return id;
            }

            return 0; // ID padrão se não encontrado
        }
    }
} 
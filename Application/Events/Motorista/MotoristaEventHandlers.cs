using Dominio.Entidades.Pessoas;
using Dominio.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Motorista
{
    public class MotoristaCriadoEventHandler : INotificationHandler<MotoristaCriadoEvent>
    {
        private readonly ILogger<MotoristaCriadoEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public MotoristaCriadoEventHandler(
            ILogger<MotoristaCriadoEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(MotoristaCriadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Motorista criado: {MotoristaId} - {Nome} - CPF: {CPF}", 
                notification.MotoristaId, notification.Nome, notification.CPF);

            await _auditoriaService.RegistrarEventoAsync(
                "MotoristaCriado",
                notification.MotoristaId,
                $"Motorista {notification.Nome} (CPF: {notification.CPF}) foi criado",
                "Sistema",
                "127.0.0.1",
                cancellationToken);

            // Aqui você pode adicionar outras ações como:
            // - Enviar email de boas-vindas
            // - Criar perfil em sistema externo
            // - Notificar sistema de RH
        }
    }

    public class MotoristaDocumentosAtualizadosEventHandler : INotificationHandler<MotoristaDocumentosAtualizadosEvent>
    {
        private readonly ILogger<MotoristaDocumentosAtualizadosEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public MotoristaDocumentosAtualizadosEventHandler(
            ILogger<MotoristaDocumentosAtualizadosEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(MotoristaDocumentosAtualizadosEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Documentos do motorista atualizados: {MotoristaId} - {Nome}", 
                notification.MotoristaId, notification.Nome);

            await _auditoriaService.RegistrarEventoAsync(
                "MotoristaDocumentosAtualizados",
                notification.MotoristaId,
                $"Documentos do motorista {notification.Nome} foram atualizados",
                "Sistema",
                "127.0.0.1",
                cancellationToken);

            // Aqui você pode adicionar outras ações como:
            // - Validar documentos em sistema externo
            // - Notificar departamento de compliance
            // - Atualizar sistema de seguros
        }
    }

    public class MotoristaCNHRenovadaEventHandler : INotificationHandler<MotoristaCNHRenovadaEvent>
    {
        private readonly ILogger<MotoristaCNHRenovadaEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public MotoristaCNHRenovadaEventHandler(
            ILogger<MotoristaCNHRenovadaEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(MotoristaCNHRenovadaEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CNH do motorista renovada: {MotoristaId} - {Nome} - Nova validade: {NovaValidade}", 
                notification.MotoristaId, notification.Nome, notification.NovaValidade);

            await _auditoriaService.RegistrarEventoAsync(
                "MotoristaCNHRenovada",
                notification.MotoristaId,
                $"CNH do motorista {notification.Nome} foi renovada até {notification.NovaValidade:dd/MM/yyyy}",
                "Sistema",
                "127.0.0.1",
                cancellationToken);

            // Aqui você pode adicionar outras ações como:
            // - Cancelar alertas de vencimento
            // - Notificar sistema de seguros
            // - Atualizar sistema de RH
            // - Enviar notificação para o motorista
        }
    }
}

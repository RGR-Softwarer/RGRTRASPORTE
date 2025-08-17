using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Passageiro
{
    public class PassageiroCriadoEventHandler : INotificationHandler<PassageiroCriadoEvent>
    {
        private readonly ILogger<PassageiroCriadoEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public PassageiroCriadoEventHandler(
            ILogger<PassageiroCriadoEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(PassageiroCriadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Passageiro criado: {PassageiroId} - {Nome} - CPF: {CPF}", 
                notification.PassageiroId, notification.Nome, notification.CPF);

            await _auditoriaService.RegistrarEventoAsync(
                "PassageiroCriado",
                notification.PassageiroId,
                $"Passageiro {notification.Nome} (CPF: {notification.CPF}) foi criado",
                "Sistema",
                "127.0.0.1",
                cancellationToken);

            // Aqui você pode adicionar outras ações como:
            // - Enviar email de boas-vindas
            // - Criar conta no app móvel
            // - Enviar SMS com informações
            // - Registrar no sistema de fidelidade
        }
    }

    public class PassageiroAtualizadoEventHandler : INotificationHandler<PassageiroAtualizadoEvent>
    {
        private readonly ILogger<PassageiroAtualizadoEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public PassageiroAtualizadoEventHandler(
            ILogger<PassageiroAtualizadoEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(PassageiroAtualizadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Passageiro atualizado: {PassageiroId} - {Nome}", 
                notification.PassageiroId, notification.Nome);

            await _auditoriaService.RegistrarEventoAsync(
                "PassageiroAtualizado",
                notification.PassageiroId,
                $"Dados do passageiro {notification.Nome} foram atualizados",
                "Sistema",
                "127.0.0.1",
                cancellationToken);

            // Aqui você pode adicionar outras ações como:
            // - Sincronizar com app móvel
            // - Atualizar sistema de fidelidade
            // - Enviar notificação de atualização
        }
    }

    public class PassageiroLocalidadesAtualizadasEventHandler : INotificationHandler<PassageiroLocalidadesAtualizadasEvent>
    {
        private readonly ILogger<PassageiroLocalidadesAtualizadasEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public PassageiroLocalidadesAtualizadasEventHandler(
            ILogger<PassageiroLocalidadesAtualizadasEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(PassageiroLocalidadesAtualizadasEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Localidades do passageiro atualizadas: {PassageiroId} - {Nome}", 
                notification.PassageiroId, notification.Nome);

            await _auditoriaService.RegistrarEventoAsync(
                "PassageiroLocalidadesAtualizadas",
                notification.PassageiroId,
                $"Localidades do passageiro {notification.Nome} foram atualizadas",
                "Sistema",
                "127.0.0.1",
                cancellationToken);

            // Aqui você pode adicionar outras ações como:
            // - Recalcular rotas preferenciais
            // - Sugerir viagens com base nas novas localidades
            // - Atualizar preferências no sistema de recomendação
        }
    }
}

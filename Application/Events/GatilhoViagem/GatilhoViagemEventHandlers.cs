using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.GatilhoViagem
{
    public class GatilhoViagemCriadoEventHandler : INotificationHandler<GatilhoViagemCriadoEvent>
    {
        private readonly ILogger<GatilhoViagemCriadoEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public GatilhoViagemCriadoEventHandler(
            ILogger<GatilhoViagemCriadoEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(GatilhoViagemCriadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Gatilho de viagem criado: {GatilhoId} - {Descricao}", 
                notification.GatilhoId, notification.Descricao);

            await _auditoriaService.RegistrarEventoAsync(
                "GatilhoViagemCriado",
                notification.GatilhoId,
                $"Gatilho de viagem '{notification.Descricao}' foi criado",
                "Sistema",
                "127.0.0.1",
                cancellationToken);

            // Aqui você pode adicionar outras ações como:
            // - Programar jobs automáticos para gerar viagens
            // - Notificar sistema de planejamento
            // - Enviar notificação para gestores
        }
    }

    public class GatilhoViagemHorarioAtualizadoEventHandler : INotificationHandler<GatilhoViagemHorarioAtualizadoEvent>
    {
        private readonly ILogger<GatilhoViagemHorarioAtualizadoEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public GatilhoViagemHorarioAtualizadoEventHandler(
            ILogger<GatilhoViagemHorarioAtualizadoEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(GatilhoViagemHorarioAtualizadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Horário do gatilho atualizado: {GatilhoId} - {Descricao}", 
                notification.GatilhoId, notification.Descricao);

            await _auditoriaService.RegistrarEventoAsync(
                "GatilhoViagemHorarioAtualizado",
                notification.GatilhoId,
                $"Horários do gatilho '{notification.Descricao}' foram atualizados",
                "Sistema",
                "127.0.0.1",
                cancellationToken);

            // Aqui você pode adicionar outras ações como:
            // - Atualizar viagens futuras já geradas
            // - Notificar passageiros sobre mudanças
            // - Recalcular escalas e conexões
        }
    }

    public class GatilhoViagemValorAtualizadoEventHandler : INotificationHandler<GatilhoViagemValorAtualizadoEvent>
    {
        private readonly ILogger<GatilhoViagemValorAtualizadoEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public GatilhoViagemValorAtualizadoEventHandler(
            ILogger<GatilhoViagemValorAtualizadoEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(GatilhoViagemValorAtualizadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Valor do gatilho atualizado: {GatilhoId} - {Descricao} - Novo valor: {NovoValor}", 
                notification.GatilhoId, notification.Descricao, notification.NovoValor);

            await _auditoriaService.RegistrarEventoAsync(
                "GatilhoViagemValorAtualizado",
                notification.GatilhoId,
                $"Valor da passagem do gatilho '{notification.Descricao}' foi atualizado para R$ {notification.NovoValor:F2}",
                "Sistema",
                "127.0.0.1",
                cancellationToken);

            // Aqui você pode adicionar outras ações como:
            // - Atualizar sistema de preços
            // - Notificar sistema financeiro
            // - Recalcular projeções de receita
        }
    }

    public class GatilhoViagemAtivadoEventHandler : INotificationHandler<GatilhoViagemAtivadoEvent>
    {
        private readonly ILogger<GatilhoViagemAtivadoEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public GatilhoViagemAtivadoEventHandler(
            ILogger<GatilhoViagemAtivadoEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(GatilhoViagemAtivadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Gatilho de viagem ativado: {GatilhoId} - {Descricao}", 
                notification.GatilhoId, notification.Descricao);

            await _auditoriaService.RegistrarEventoAsync(
                "GatilhoViagemAtivado",
                notification.GatilhoId,
                $"Gatilho de viagem '{notification.Descricao}' foi ativado",
                "Sistema",
                "127.0.0.1",
                cancellationToken);

            // Aqui você pode adicionar outras ações como:
            // - Programar geração automática de viagens
            // - Ativar vendas online
            // - Notificar sistema de marketing
        }
    }

    public class GatilhoViagemDesativadoEventHandler : INotificationHandler<GatilhoViagemDesativadoEvent>
    {
        private readonly ILogger<GatilhoViagemDesativadoEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public GatilhoViagemDesativadoEventHandler(
            ILogger<GatilhoViagemDesativadoEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(GatilhoViagemDesativadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Gatilho de viagem desativado: {GatilhoId} - {Descricao}", 
                notification.GatilhoId, notification.Descricao);

            await _auditoriaService.RegistrarEventoAsync(
                "GatilhoViagemDesativado",
                notification.GatilhoId,
                $"Gatilho de viagem '{notification.Descricao}' foi desativado",
                "Sistema",
                "127.0.0.1",
                cancellationToken);

            // Aqui você pode adicionar outras ações como:
            // - Cancelar jobs automáticos
            // - Suspender vendas online
            // - Notificar passageiros sobre cancelamentos
        }
    }

    public class ViagemGeradaPorGatilhoEventHandler : INotificationHandler<ViagemGeradaPorGatilhoEvent>
    {
        private readonly ILogger<ViagemGeradaPorGatilhoEventHandler> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public ViagemGeradaPorGatilhoEventHandler(
            ILogger<ViagemGeradaPorGatilhoEventHandler> logger,
            IAuditoriaService auditoriaService)
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
        }

        public async Task Handle(ViagemGeradaPorGatilhoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Viagem gerada por gatilho: Gatilho {GatilhoId} - Viagem {ViagemId} - Data: {DataViagem}", 
                notification.GatilhoId, notification.ViagemId, notification.DataViagem);

            await _auditoriaService.RegistrarEventoAsync(
                "ViagemGeradaPorGatilho",
                notification.ViagemId,
                $"Viagem {notification.ViagemId} foi gerada automaticamente pelo gatilho {notification.GatilhoId} para {notification.DataViagem:dd/MM/yyyy}",
                "Sistema",
                "127.0.0.1",
                cancellationToken);

            // Aqui você pode adicionar outras ações como:
            // - Notificar sistema de vendas
            // - Abrir reservas automaticamente
            // - Enviar notificações push para app móvel
            // - Atualizar dashboard de operações
        }
    }
}

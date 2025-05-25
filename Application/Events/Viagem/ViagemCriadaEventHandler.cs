using Dominio.Interfaces.Hangfire;
using Hangfire;
using MediatR;

namespace Application.Events.Viagem
{
    public class ViagemCriadaEventHandler : INotificationHandler<ViagemCriadaEvent>
    {

        public Task Handle(ViagemCriadaEvent notification, CancellationToken cancellationToken)
        {
            BackgroundJob.Enqueue<IProcessadorDeEventoService>(job =>
                job.ProcessarViagemCriada(new Dominio.Dtos.Viagens.ViagemCriadaJobData()
                {
                    TenantId = notification.TenantId,
                    ViagemId = notification.ViagemId,
                }));

            return Task.CompletedTask;
        }

    }
}

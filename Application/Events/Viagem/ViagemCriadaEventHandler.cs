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
                job.ProcessarViagemCriada(notification.ViagemId));

            return Task.CompletedTask;
        }

    }
}

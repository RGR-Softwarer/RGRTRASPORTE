using Dominio.Interfaces.Hangfire;
using Hangfire;
using MediatR;

namespace Application.Events.Viagem
{
    public class ViagemAtualizadaEventHandler : INotificationHandler<ViagemAtualizadaEvent>
    {
        public Task Handle(ViagemAtualizadaEvent notification, CancellationToken cancellationToken)
        {
            BackgroundJob.Enqueue<IProcessadorDeEventoService>(job =>
                job.ProcessarViagemAtualizada(new Dominio.Dtos.Viagens.ViagemAtualizadaJobData()
                {
                    ViagemId = notification.ViagemId,
                }));

            return Task.CompletedTask;
        }
    }
} 
using MediatR;
using Dominio.Interfaces.Service.Viagens;

namespace Application.Events.Viagem;

public class ViagemFinalizadaEventHandler : INotificationHandler<Dominio.Events.Viagem.ViagemFinalizadaEvent>
{
    private readonly IViagemService _viagemService;

    public ViagemFinalizadaEventHandler(IViagemService viagemService)
    {
        _viagemService = viagemService;
    }

    public async Task Handle(Dominio.Events.Viagem.ViagemFinalizadaEvent notification, CancellationToken cancellationToken)
    {
        await _viagemService.FinalizarViagemAsync(notification.ViagemId, notification.UsuarioOperacao);
    }
} 
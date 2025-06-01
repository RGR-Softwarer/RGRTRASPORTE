using MediatR;
using Dominio.Events.Viagem;
using Dominio.Interfaces.Service.Viagens;

namespace Application.Events.Viagem;

public class ViagemRemovidaEventHandler : INotificationHandler<ViagemRemovidaEvent>
{
    private readonly IViagemService _viagemService;

    public ViagemRemovidaEventHandler(IViagemService viagemService)
    {
        _viagemService = viagemService;
    }

    public async Task Handle(ViagemRemovidaEvent notification, CancellationToken cancellationToken)
    {
        await _viagemService.RemoverAsync(notification.ViagemId);
    }
} 
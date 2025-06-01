using MediatR;
using Dominio.Interfaces.Service.Viagens;

namespace Application.Events.Viagem;

public class ViagemIniciadaEventHandler : INotificationHandler<Dominio.Events.Viagem.ViagemIniciadaEvent>
{
    private readonly IViagemService _viagemService;

    public ViagemIniciadaEventHandler(IViagemService viagemService)
    {
        _viagemService = viagemService;
    }

    public async Task Handle(Dominio.Events.Viagem.ViagemIniciadaEvent notification, CancellationToken cancellationToken)
    {
        await _viagemService.IniciarViagemAsync(notification.ViagemId, notification.UsuarioOperacao);
    }
} 
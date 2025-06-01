using MediatR;

namespace Dominio.Events.Viagem
{
    public interface IViagemEvent : INotification
    {
        long ViagemId { get; }
    }
} 
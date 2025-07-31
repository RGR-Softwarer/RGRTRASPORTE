using MediatR;

namespace Application.Events.Viagem
{
    public class ViagemRemovidaEvent : INotification
    {
        public long ViagemId { get; }

        public ViagemRemovidaEvent(long viagemId)
        {
            ViagemId = viagemId;
        }
    }
} 

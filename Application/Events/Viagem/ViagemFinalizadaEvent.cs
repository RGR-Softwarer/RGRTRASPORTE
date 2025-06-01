using MediatR;

namespace Application.Events.Viagem
{
    public class ViagemFinalizadaEvent : INotification
    {
        public long ViagemId { get; }

        public ViagemFinalizadaEvent(long viagemId)
        {
            ViagemId = viagemId;
        }
    }
} 
using MediatR;

namespace Application.Events.Viagem
{
    public class ViagemIniciadaEvent : INotification
    {
        public long ViagemId { get; }

        public ViagemIniciadaEvent(long viagemId)
        {
            ViagemId = viagemId;
        }
    }
} 
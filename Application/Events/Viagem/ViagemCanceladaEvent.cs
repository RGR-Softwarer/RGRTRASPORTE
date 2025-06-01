using MediatR;

namespace Application.Events.Viagem
{
    public class ViagemCanceladaEvent : INotification
    {
        public long ViagemId { get; }

        public ViagemCanceladaEvent(long viagemId)
        {
            ViagemId = viagemId;
        }
    }
} 
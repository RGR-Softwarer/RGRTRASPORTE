using MediatR;

namespace Application.Events.Viagem
{
    public class ViagemCriadaEvent : INotification
    {
        public long ViagemId { get; }
        public string TenantId { get; }

        public ViagemCriadaEvent(long viagemId)
        {
            ViagemId = viagemId;
        }
    }
}

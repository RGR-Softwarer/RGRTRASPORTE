using Dominio.Events.Base;
using MediatR;

namespace Dominio.Events.Viagens
{
    public class ViagemAtualizadaEvent : DomainEvent, INotification
    {
        public long ViagemId { get; private set; }
        public DateTime DataAtualizacao { get; private set; }

        public ViagemAtualizadaEvent(long viagemId)
        {
            ViagemId = viagemId;
            DataAtualizacao = DateTime.UtcNow;
        }
    }
} 
using Dominio.Events.Base;

namespace Dominio.Events.Viagens
{
    public class PassageiroRemovidoEvent : DomainEvent
    {
        public long ViagemId { get; private set; }
        public long PassageiroId { get; private set; }
        public DateTime DataRemocao { get; private set; }

        public PassageiroRemovidoEvent(long viagemId, long passageiroId)
        {
            ViagemId = viagemId;
            PassageiroId = passageiroId;
            DataRemocao = DateTime.UtcNow;
        }
    }
} 

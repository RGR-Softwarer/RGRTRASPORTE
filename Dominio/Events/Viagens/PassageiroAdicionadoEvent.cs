using Dominio.Events.Base;

namespace Dominio.Events.Viagens
{
    public class PassageiroAdicionadoEvent : DomainEvent
    {
        public long ViagemId { get; private set; }
        public long PassageiroId { get; private set; }
        public DateTime DataAdicao { get; private set; }

        public PassageiroAdicionadoEvent(long viagemId, long passageiroId)
        {
            ViagemId = viagemId;
            PassageiroId = passageiroId;
            DataAdicao = DateTime.UtcNow;
        }
    }
} 

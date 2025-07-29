using Dominio.Events.Base;

namespace Dominio.Events.Viagens
{
    public class ViagemLotadaEvent : DomainEvent
    {
        public long ViagemId { get; }
        public int QuantidadePassageiros { get; }

        public ViagemLotadaEvent(long viagemId, int quantidadePassageiros)
        {
            ViagemId = viagemId;
            QuantidadePassageiros = quantidadePassageiros;
        }
    }
} 
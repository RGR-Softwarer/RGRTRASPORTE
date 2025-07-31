using Dominio.Events.Base;

namespace Dominio.Events.Viagens
{
    public class ViagemComVagasDisponiveisEvent : DomainEvent
    {
        public long ViagemId { get; }
        public int VagasDisponiveis { get; }

        public ViagemComVagasDisponiveisEvent(long viagemId, int vagasDisponiveis)
        {
            ViagemId = viagemId;
            VagasDisponiveis = vagasDisponiveis;
        }
    }
} 

using Dominio.Events.Base;

namespace Dominio.Events.Viagens
{
    public class ViagemCanceladaEvent : DomainEvent
    {
        public long ViagemId { get; }
        public string Motivo { get; }

        public ViagemCanceladaEvent(long viagemId, string motivo)
        {
            ViagemId = viagemId;
            Motivo = motivo;
        }
    }
} 

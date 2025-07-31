using MediatR;
using Dominio.Events.Viagem;

namespace Application.Events.Viagem
{
    public class ViagemAtualizadaEvent : IViagemEvent
    {
        public long ViagemId { get; }

        public ViagemAtualizadaEvent(long viagemId)
        {
            ViagemId = viagemId;
        }
    }
} 

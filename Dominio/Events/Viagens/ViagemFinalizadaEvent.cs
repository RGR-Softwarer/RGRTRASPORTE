using Dominio.Events.Base;
using MediatR;

namespace Dominio.Events.Viagens;

public class ViagemFinalizadaEvent : DomainEvent, INotification
{
    public long ViagemId { get; }
    public DateTime DataFinalizacao { get; }

    public ViagemFinalizadaEvent(long viagemId, DateTime dataFinalizacao)
    {
        ViagemId = viagemId;
        DataFinalizacao = dataFinalizacao;
    }
}

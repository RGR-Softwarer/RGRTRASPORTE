using Dominio.Events.Base;
using MediatR;

namespace Dominio.Events.Viagens;

public class ViagemIniciadaEvent : DomainEvent, INotification
{
    public long ViagemId { get; }
    public DateTime DataInicio { get; }

    public ViagemIniciadaEvent(long viagemId, DateTime dataInicio)
    {
        ViagemId = viagemId;
        DataInicio = dataInicio;
    }
}
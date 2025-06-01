using Application.Events.Base;

namespace Application.Events.Viagem;

public class ViagemCriadaEvent : BaseEvent
{
    public Guid ViagemId { get; private set; }
    public DateTime DataViagem { get; private set; }
    public Guid VeiculoId { get; private set; }
    public Guid MotoristaId { get; private set; }
    public string Origem { get; private set; }
    public string Destino { get; private set; }
    public decimal ValorViagem { get; private set; }
    public List<Guid> PassageirosIds { get; private set; }

    public ViagemCriadaEvent(
        Guid viagemId,
        DateTime dataViagem,
        Guid veiculoId,
        Guid motoristaId,
        string origem,
        string destino,
        decimal valorViagem,
        List<Guid> passageirosIds)
    {
        ViagemId = viagemId;
        DataViagem = dataViagem;
        VeiculoId = veiculoId;
        MotoristaId = motoristaId;
        Origem = origem;
        Destino = destino;
        ValorViagem = valorViagem;
        PassageirosIds = passageirosIds;
    }
}

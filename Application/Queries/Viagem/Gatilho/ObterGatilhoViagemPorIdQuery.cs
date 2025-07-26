using MediatR;

namespace Application.Queries.Viagem.Gatilho;

public class ObterGatilhoViagemPorIdQuery : IRequest<GatilhoViagemDto>
{
    public long Id { get; }

    public ObterGatilhoViagemPorIdQuery(long id)
    {
        Id = id;
    }
} 
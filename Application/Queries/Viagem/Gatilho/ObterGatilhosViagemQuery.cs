using MediatR;

namespace Application.Queries.Viagem.Gatilho;

public class ObterGatilhosViagemQuery : IRequest<IEnumerable<GatilhoViagemDto>>
{
} 
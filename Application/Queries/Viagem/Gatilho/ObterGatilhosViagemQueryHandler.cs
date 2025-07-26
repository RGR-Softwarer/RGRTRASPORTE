using MediatR;
using AutoMapper;
using Dominio.Interfaces.Infra.Data.Viagens.Gatilho;

namespace Application.Queries.Viagem.Gatilho;

public class ObterGatilhosViagemQueryHandler : IRequestHandler<ObterGatilhosViagemQuery, IEnumerable<GatilhoViagemDto>>
{
    private readonly IGatilhoViagemRepository _repository;
    private readonly IMapper _mapper;

    public ObterGatilhosViagemQueryHandler(IGatilhoViagemRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GatilhoViagemDto>> Handle(ObterGatilhosViagemQuery request, CancellationToken cancellationToken)
    {
        var gatilhos = await _repository.ObterTodosAsync();
        return _mapper.Map<IEnumerable<GatilhoViagemDto>>(gatilhos);
    }
} 
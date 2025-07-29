using MediatR;
using AutoMapper;
using Dominio.Interfaces.Infra.Data;
using GatilhoViagemEntity = Dominio.Entidades.Viagens.Gatilho.GatilhoViagem;

namespace Application.Queries.Viagem.Gatilho;

public class ObterGatilhosViagemQueryHandler : IRequestHandler<ObterGatilhosViagemQuery, IEnumerable<GatilhoViagemDto>>
{
    private readonly IGenericRepository<GatilhoViagemEntity> _repository;
    private readonly IMapper _mapper;

    public ObterGatilhosViagemQueryHandler(IGenericRepository<GatilhoViagemEntity> repository, IMapper mapper)
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
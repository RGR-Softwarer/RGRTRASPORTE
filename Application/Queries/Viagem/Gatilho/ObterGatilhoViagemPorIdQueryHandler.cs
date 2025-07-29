using MediatR;
using AutoMapper;
using Dominio.Interfaces.Infra.Data;
using GatilhoViagemEntity = Dominio.Entidades.Viagens.Gatilho.GatilhoViagem;

namespace Application.Queries.Viagem.Gatilho;

public class ObterGatilhoViagemPorIdQueryHandler : IRequestHandler<ObterGatilhoViagemPorIdQuery, GatilhoViagemDto>
{
    private readonly IGenericRepository<GatilhoViagemEntity> _repository;
    private readonly IMapper _mapper;

    public ObterGatilhoViagemPorIdQueryHandler(IGenericRepository<GatilhoViagemEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GatilhoViagemDto> Handle(ObterGatilhoViagemPorIdQuery request, CancellationToken cancellationToken)
    {
        var gatilho = await _repository.ObterPorIdAsync(request.Id);
        return _mapper.Map<GatilhoViagemDto>(gatilho);
    }
} 
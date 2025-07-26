using MediatR;
using AutoMapper;
using Dominio.Interfaces.Infra.Data.Viagens.Gatilho;

namespace Application.Queries.Viagem.Gatilho;

public class ObterGatilhoViagemPorIdQueryHandler : IRequestHandler<ObterGatilhoViagemPorIdQuery, GatilhoViagemDto>
{
    private readonly IGatilhoViagemRepository _repository;
    private readonly IMapper _mapper;

    public ObterGatilhoViagemPorIdQueryHandler(IGatilhoViagemRepository repository, IMapper mapper)
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
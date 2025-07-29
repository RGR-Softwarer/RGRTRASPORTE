using MediatR;
using Application.Common;
using Dominio.Interfaces.Infra.Data;
using GatilhoViagemEntity = Dominio.Entidades.Viagens.Gatilho.GatilhoViagem;

namespace Application.Commands.Viagem.Gatilho;

public class CriarGatilhoViagemCommandHandler : IRequestHandler<CriarGatilhoViagemCommand, BaseResponse<long>>
{
    private readonly IGenericRepository<GatilhoViagemEntity> _repository;

    public CriarGatilhoViagemCommandHandler(IGenericRepository<GatilhoViagemEntity> repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponse<long>> Handle(CriarGatilhoViagemCommand request, CancellationToken cancellationToken)
    {
        //var gatilho = new GatilhoViagem(request.ViagemId, request.Descricao);
        //await _repository.AdicionarAsync(gatilho);
        //return BaseResponse<long>.Ok(gatilho.Id);
        return BaseResponse<long>.Ok(0);
    }
} 
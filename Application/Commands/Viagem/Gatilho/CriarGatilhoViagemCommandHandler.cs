using MediatR;
using Application.Common;
using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens.Gatilho;
using Dominio.Entidades.Viagens.Gatilho;

namespace Application.Commands.Viagem.Gatilho;

public class CriarGatilhoViagemCommandHandler : IRequestHandler<CriarGatilhoViagemCommand, BaseResponse<long>>
{
    private readonly IGatilhoViagemRepository _repository;

    public CriarGatilhoViagemCommandHandler(IGatilhoViagemRepository repository)
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
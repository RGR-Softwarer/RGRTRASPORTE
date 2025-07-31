using MediatR;
using Application.Common;
using Dominio.Interfaces.Infra.Data;
using GatilhoViagemEntity = Dominio.Entidades.Viagens.Gatilho.GatilhoViagem;

namespace Application.Commands.Viagem.Gatilho;

public class RemoverGatilhoViagemCommandHandler : IRequestHandler<RemoverGatilhoViagemCommand, BaseResponse<bool>>
{
    private readonly IGenericRepository<GatilhoViagemEntity> _repository;

    public RemoverGatilhoViagemCommandHandler(IGenericRepository<GatilhoViagemEntity> repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponse<bool>> Handle(RemoverGatilhoViagemCommand request, CancellationToken cancellationToken)
    {
        var gatilho = await _repository.ObterPorIdAsync(request.Id);
        if (gatilho == null)
            return BaseResponse<bool>.Erro("Gatilho não encontrado");

        await _repository.RemoverAsync(gatilho);
        return BaseResponse<bool>.Ok(true);
    }
} 

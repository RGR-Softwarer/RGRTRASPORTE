using MediatR;
using Application.Common;
using Dominio.Interfaces.Infra.Data;
using GatilhoViagemEntity = Dominio.Entidades.Viagens.Gatilho.GatilhoViagem;

namespace Application.Commands.Viagem.Gatilho;

public class AtualizarGatilhoViagemCommandHandler : IRequestHandler<AtualizarGatilhoViagemCommand, BaseResponse<bool>>
{
    private readonly IGenericRepository<GatilhoViagemEntity> _repository;

    public AtualizarGatilhoViagemCommandHandler(IGenericRepository<GatilhoViagemEntity> repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponse<bool>> Handle(AtualizarGatilhoViagemCommand request, CancellationToken cancellationToken)
    {
        var gatilho = await _repository.ObterPorIdAsync(request.Id);
        if (gatilho == null)
            return BaseResponse<bool>.Erro("Gatilho não encontrado");

        //gatilho.Atualizar(request.Descricao);
        await _repository.AtualizarAsync(gatilho);
        return BaseResponse<bool>.Ok(true);
    }
} 

using MediatR;
using Application.Common;
using Dominio.Interfaces.Infra.Data.Viagens.Gatilho;

namespace Application.Commands.Viagem.Gatilho;

public class AtualizarGatilhoViagemCommandHandler : IRequestHandler<AtualizarGatilhoViagemCommand, BaseResponse<bool>>
{
    private readonly IGatilhoViagemRepository _repository;

    public AtualizarGatilhoViagemCommandHandler(IGatilhoViagemRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponse<bool>> Handle(AtualizarGatilhoViagemCommand request, CancellationToken cancellationToken)
    {
        var gatilho = await _repository.ObterGatilhoCompletoPorIdAsync(request.Id);
        if (gatilho == null)
            return BaseResponse<bool>.Erro("Gatilho não encontrado");

        //gatilho.Atualizar(request.Descricao);
        await _repository.AtualizarAsync(gatilho);
        return BaseResponse<bool>.Ok(true);
    }
} 
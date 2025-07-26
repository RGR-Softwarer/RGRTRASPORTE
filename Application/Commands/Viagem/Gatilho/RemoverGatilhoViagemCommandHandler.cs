using MediatR;
using Application.Common;
using Dominio.Interfaces.Infra.Data.Viagens.Gatilho;

namespace Application.Commands.Viagem.Gatilho;

public class RemoverGatilhoViagemCommandHandler : IRequestHandler<RemoverGatilhoViagemCommand, BaseResponse<bool>>
{
    private readonly IGatilhoViagemRepository _repository;

    public RemoverGatilhoViagemCommandHandler(IGatilhoViagemRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponse<bool>> Handle(RemoverGatilhoViagemCommand request, CancellationToken cancellationToken)
    {
        var gatilho = await _repository.ObterGatilhoCompletoPorIdAsync(request.Id);
        if (gatilho == null)
            return BaseResponse<bool>.Erro("Gatilho não encontrado");

        await _repository.RemoverAsync(gatilho);
        return BaseResponse<bool>.Ok(true);
    }
} 
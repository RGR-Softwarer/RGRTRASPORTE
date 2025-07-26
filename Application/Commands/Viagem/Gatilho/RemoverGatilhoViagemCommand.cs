using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem.Gatilho;

public class RemoverGatilhoViagemCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }

    public RemoverGatilhoViagemCommand(
        long id,
        string usuarioId,
        string usuarioCriacao) : base(usuarioId, usuarioCriacao)
    {
        Id = id;
    }
} 
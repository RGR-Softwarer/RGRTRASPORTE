using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Passageiro;

public class RemoverPassageiroCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }

    public RemoverPassageiroCommand(
        long id,
        string usuarioId,
        string usuarioCriacao) : base(usuarioId, usuarioCriacao)
    {
        Id = id;
    }
} 
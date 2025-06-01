using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem;

public class FinalizarViagemCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }

    public FinalizarViagemCommand(
        long id,
        string usuarioId,
        string usuarioCriacao) : base(usuarioId, usuarioCriacao)
    {
        Id = id;
    }
} 
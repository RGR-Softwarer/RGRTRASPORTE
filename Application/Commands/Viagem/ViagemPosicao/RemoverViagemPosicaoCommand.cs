using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem.ViagemPosicao;

public class RemoverViagemPosicaoCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }

    public RemoverViagemPosicaoCommand(
        long id,
        string usuarioId,
        string usuarioCriacao) : base(usuarioId, usuarioCriacao)
    {
        Id = id;
    }
} 
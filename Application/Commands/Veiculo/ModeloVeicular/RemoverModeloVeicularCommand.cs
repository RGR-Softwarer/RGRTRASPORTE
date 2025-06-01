using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Veiculo.ModeloVeicular;

public class RemoverModeloVeicularCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }

    public RemoverModeloVeicularCommand(
        long id,
        string usuarioId,
        string usuarioCriacao) : base(usuarioId, usuarioCriacao)
    {
        Id = id;
    }
} 
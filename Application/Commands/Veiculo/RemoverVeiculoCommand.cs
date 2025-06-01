using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Veiculo;

public class RemoverVeiculoCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }

    public RemoverVeiculoCommand(long id, string usuarioId, string usuarioCriacao) : base(usuarioId, usuarioCriacao)
    {
        Id = id;
    }
} 
using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem;

public class CancelarViagemCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }
    public string Motivo { get; private set; }

    public CancelarViagemCommand(
        long id,
        string motivo,
        string usuarioId,
        string usuarioCriacao) : base(usuarioId, usuarioCriacao)
    {
        Id = id;
        Motivo = motivo;
    }
} 
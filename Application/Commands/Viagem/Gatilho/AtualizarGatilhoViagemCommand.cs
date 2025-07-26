using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem.Gatilho;

public class AtualizarGatilhoViagemCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }
    public string Descricao { get; private set; }

    public AtualizarGatilhoViagemCommand(
        long id,
        string descricao,
        string usuarioId,
        string usuarioCriacao) : base(usuarioId, usuarioCriacao)
    {
        Id = id;
        Descricao = descricao;
    }
} 
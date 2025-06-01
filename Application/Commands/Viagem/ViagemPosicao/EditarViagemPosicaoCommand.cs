using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem.ViagemPosicao;

public class EditarViagemPosicaoCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }
    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }
    public DateTime DataPosicao { get; private set; }

    public EditarViagemPosicaoCommand(
        long id,
        decimal latitude,
        decimal longitude,
        DateTime dataPosicao,
        string usuarioId,
        string usuarioCriacao) : base(usuarioId, usuarioCriacao)
    {
        Id = id;
        Latitude = latitude;
        Longitude = longitude;
        DataPosicao = dataPosicao;
    }
}
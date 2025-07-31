using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem.ViagemPosicao;

public class AdicionarViagemPosicaoCommand : BaseCommand<BaseResponse<long>>
{
    public long ViagemId { get; private set; }
    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }
    public DateTime DataPosicao { get; private set; }

    public AdicionarViagemPosicaoCommand(
        long viagemId,
        decimal latitude,
        decimal longitude,
        DateTime dataHora)
    {
        ViagemId = viagemId;
        Latitude = latitude;
        Longitude = longitude;
        DataPosicao = dataHora;
    }
} 

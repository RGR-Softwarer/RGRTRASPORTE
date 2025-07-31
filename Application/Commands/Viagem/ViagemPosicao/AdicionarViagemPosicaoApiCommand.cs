using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem.ViagemPosicao;

public class AdicionarViagemPosicaoApiCommand : BaseCommand<BaseResponse<long>>
{
    public long ViagemId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public DateTime DataPosicao { get; set; }

    public AdicionarViagemPosicaoApiCommand()
    {
        // Construtor padrão para model binding
    }

    public AdicionarViagemPosicaoApiCommand(long viagemId, decimal latitude, decimal longitude, DateTime dataPosicao)
    {
        ViagemId = viagemId;
        Latitude = latitude;
        Longitude = longitude;
        DataPosicao = dataPosicao;
    }
} 

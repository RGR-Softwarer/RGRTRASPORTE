using Application.Commands.Base;
using Application.Common;
using System.Text.Json.Serialization;

namespace Application.Commands.Viagem.ViagemPosicao;

public class AdicionarViagemPosicaoApiCommand : BaseCommand<BaseResponse<long>>
{
    public long ViagemId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    
    [JsonPropertyName("dataPosicao")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? DataPosicao { get; set; }
    
    [JsonPropertyName("dataHora")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? DataHora { get; set; }

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
    
    // Propriedade calculada para obter a data/hora (aceita ambos os formatos)
    public DateTime GetDataPosicao()
    {
        return DataPosicao ?? DataHora ?? DateTime.UtcNow;
    }
} 

using Application.Common;
using Application.Queries.Base;
using Application.Queries.Viagem.Models;

namespace Application.Queries.Viagem.ViagemPosicao;

public class ObterViagemPosicoesQuery : BaseQuery<BaseResponse<IEnumerable<ViagemPosicaoDto>>>
{
    public long ViagemId { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }

    public ObterViagemPosicoesQuery()
    {
    }

    public ObterViagemPosicoesQuery(long viagemId, DateTime? dataInicio = null, DateTime? dataFim = null)
    {
        ViagemId = viagemId;
        DataInicio = dataInicio;
        DataFim = dataFim;
    }
} 
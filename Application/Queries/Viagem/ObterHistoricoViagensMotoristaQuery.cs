using Application.Common;
using Application.Queries.Base;
using Application.Queries.Viagem.Models;

namespace Application.Queries.Viagem;

public class ObterHistoricoViagensMotoristaQuery : BaseQuery<BaseResponse<IEnumerable<ViagemDto>>>
{
    public long MotoristaId { get; private set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }

    public ObterHistoricoViagensMotoristaQuery(long motoristaId, DateTime? dataInicio = null, DateTime? dataFim = null)
    {
        MotoristaId = motoristaId;
        DataInicio = dataInicio;
        DataFim = dataFim;
    }
}





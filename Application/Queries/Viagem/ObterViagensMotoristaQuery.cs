using Application.Common;
using Application.Queries.Base;
using Application.Queries.Viagem.Models;

namespace Application.Queries.Viagem;

public class ObterViagensMotoristaQuery : BaseQuery<BaseResponse<IEnumerable<ViagemDto>>>
{
    public long MotoristaId { get; private set; }
    public DateTime? Data { get; set; }

    public ObterViagensMotoristaQuery(long motoristaId, DateTime? data = null)
    {
        MotoristaId = motoristaId;
        Data = data ?? DateTime.UtcNow.Date;
    }
}



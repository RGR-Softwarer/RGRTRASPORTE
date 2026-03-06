using Application.Common;
using Application.Queries.Base;
using Application.Queries.Viagem.Models;

namespace Application.Queries.Dashboard;

public class ObterViagensHojeQuery : BaseQuery<BaseResponse<IEnumerable<ViagemDto>>>
{
    public ObterViagensHojeQuery() { }
}



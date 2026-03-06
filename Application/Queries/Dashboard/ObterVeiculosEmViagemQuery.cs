using Application.Common;
using Application.Queries.Base;
using Application.Queries.Veiculo.Models;

namespace Application.Queries.Dashboard;

public class ObterVeiculosEmViagemQuery : BaseQuery<BaseResponse<IEnumerable<VeiculoDto>>>
{
    public ObterVeiculosEmViagemQuery() { }
}



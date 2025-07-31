using Application.Common;
using Application.Queries.Base;
using Application.Queries.Veiculo.Models;

namespace Application.Queries.Veiculo;

public class ObterVeiculoPorIdQuery : BaseQuery<BaseResponse<VeiculoDto>>
{
    public long Id { get; private set; }

    public ObterVeiculoPorIdQuery(long id)
    {
        Id = id;
    }
} 

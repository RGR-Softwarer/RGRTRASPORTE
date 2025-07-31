using Application.Common;
using Application.Queries.Base;
using Application.Queries.Veiculo.ModeloVeicular.Models;

namespace Application.Queries.Veiculo.ModeloVeicular;

public class ObterModeloVeicularPorIdQuery : BaseQuery<BaseResponse<ModeloVeicularDto>>
{
    public long Id { get; private set; }

    public ObterModeloVeicularPorIdQuery(long id)
    {
        Id = id;
    }
} 

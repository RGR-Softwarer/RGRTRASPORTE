using Application.Common;
using Application.Queries.Base;
using Application.Queries.Viagem.Models;

namespace Application.Queries.Viagem;

public class ObterViagemPorIdQuery : BaseQuery<BaseResponse<ViagemDto>>
{
    public long Id { get; private set; }

    public ObterViagemPorIdQuery(long id)
    {
        Id = id;
    }
} 
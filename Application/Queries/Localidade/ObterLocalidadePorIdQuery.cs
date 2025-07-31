using Application.Common;
using Application.Queries.Base;
using Application.Queries.Localidade.Models;

namespace Application.Queries.Localidade;

public class ObterLocalidadePorIdQuery : BaseQuery<BaseResponse<LocalidadeDto>>
{
    public long Id { get; private set; }

    public ObterLocalidadePorIdQuery(long id)
    {
        Id = id;
    }
} 

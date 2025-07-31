using Application.Common;
using Application.Queries.Base;
using Application.Queries.Localidade.Models;

namespace Application.Queries.Localidade;

public class ObterLocalidadesQuery : BaseQuery<BaseResponse<IEnumerable<LocalidadeDto>>>
{
    public string Nome { get; set; }
    public string Estado { get; set; }
    public bool? Ativo { get; set; }

    public ObterLocalidadesQuery()
    {
    }
} 

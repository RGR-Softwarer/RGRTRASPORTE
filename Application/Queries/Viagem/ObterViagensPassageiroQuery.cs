using Application.Common;
using Application.Queries.Base;
using Application.Queries.Viagem.Models;

namespace Application.Queries.Viagem;

public class ObterViagensPassageiroQuery : BaseQuery<BaseResponse<IEnumerable<ViagemDto>>>
{
    public long PassageiroId { get; private set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }

    public ObterViagensPassageiroQuery(
        long passageiroId,
        DateTime? dataInicio = null,
        DateTime? dataFim = null)
    {
        PassageiroId = passageiroId;
        DataInicio = dataInicio;
        DataFim = dataFim;
    }
}



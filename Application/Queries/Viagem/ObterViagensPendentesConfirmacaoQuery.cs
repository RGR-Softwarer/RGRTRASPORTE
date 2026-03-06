using Application.Common;
using Application.Queries.Base;
using Application.Queries.Viagem.Models;

namespace Application.Queries.Viagem;

public class ObterViagensPendentesConfirmacaoQuery : BaseQuery<BaseResponse<IEnumerable<ViagemDto>>>
{
    public long PassageiroId { get; private set; }

    public ObterViagensPendentesConfirmacaoQuery(long passageiroId)
    {
        PassageiroId = passageiroId;
    }
}



using Application.Common;
using Application.Queries.Base;
using Application.Queries.Passageiro.Models;

namespace Application.Queries.Passageiro;

public class ObterPassageiroPorIdQuery : BaseQuery<BaseResponse<PassageiroDto>>
{
    public long Id { get; private set; }
    public bool Auditado { get; private set; }

    public ObterPassageiroPorIdQuery(long id, bool auditado = false)
    {
        Id = id;
        Auditado = auditado;
    }
} 
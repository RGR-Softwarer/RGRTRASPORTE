using Application.Common;
using Application.Queries.Base;
using Application.Queries.Passageiro.Models;

namespace Application.Queries.Passageiro;

public class ObterPassageirosQuery : BaseQuery<BaseResponse<IEnumerable<PassageiroDto>>>
{
    public string? Nome { get; set; }
    public string? CPF { get; set; }
    public bool? Situacao { get; set; }

    public ObterPassageirosQuery()
    {
    }

    public ObterPassageirosQuery(string? nome = null, string? cpf = null, bool? situacao = null)
    {
        Nome = nome;
        CPF = cpf;
        Situacao = situacao;
    }
} 

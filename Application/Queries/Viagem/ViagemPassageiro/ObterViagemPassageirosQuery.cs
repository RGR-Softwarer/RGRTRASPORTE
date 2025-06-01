using Application.Common;
using Application.Queries.Base;
using Application.Queries.Viagem.Models;

namespace Application.Queries.Viagem.ViagemPassageiro;

public class ObterViagemPassageirosQuery : BaseQuery<BaseResponse<IEnumerable<ViagemPassageiroDto>>>
{
    public long ViagemId { get; set; }
    public string? NomePassageiro { get; set; }
    public string? CPFPassageiro { get; set; }
    public bool? Ativo { get; set; }

    public ObterViagemPassageirosQuery()
    {
    }

    public ObterViagemPassageirosQuery(
        long viagemId,
        string? nomePassageiro = null,
        string? cpfPassageiro = null,
        bool? ativo = null)
    {
        ViagemId = viagemId;
        NomePassageiro = nomePassageiro;
        CPFPassageiro = cpfPassageiro;
        Ativo = ativo;
    }
} 
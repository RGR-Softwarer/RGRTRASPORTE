using Application.Common;
using Application.Queries.Base;
using Application.Queries.Viagem.Models;

namespace Application.Queries.Viagem;

public class ObterViagensQuery : BaseQuery<BaseResponse<IEnumerable<ViagemDto>>>
{
    public ObterViagensQuery() { } // Construtor parameterless para model binding

    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public long? LocalidadeOrigemId { get; set; }
    public long? LocalidadeDestinoId { get; set; }
    public bool? Ativo { get; set; }

    public ObterViagensQuery(
        DateTime? dataInicio = null,
        DateTime? dataFim = null,
        long? localidadeOrigemId = null,
        long? localidadeDestinoId = null,
        bool? ativo = null)
    {
        DataInicio = dataInicio;
        DataFim = dataFim;
        LocalidadeOrigemId = localidadeOrigemId;
        LocalidadeDestinoId = localidadeDestinoId;
        Ativo = ativo;
    }
} 

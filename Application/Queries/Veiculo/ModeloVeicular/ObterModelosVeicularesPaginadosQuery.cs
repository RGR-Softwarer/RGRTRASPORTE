using Application.Common;
using Application.Queries.Base;
using Application.Queries.Veiculo.ModeloVeicular.Models;

namespace Application.Queries.Veiculo.ModeloVeicular;

public class ObterModelosVeicularesPaginadosQuery : QueryPaginadoBase<ModeloVeicularDto>
{
    public ObterModelosVeicularesPaginadosQuery()
    {
    }

    public ObterModelosVeicularesPaginadosQuery(
        List<FiltroGrid> filtros,
        int paginaAtual = 1,
        int tamanhoPagina = 10,
        string campoOrdenacao = "Id",
        bool descendente = false) : base(filtros, paginaAtual, tamanhoPagina, campoOrdenacao, descendente)
    {
    }
} 

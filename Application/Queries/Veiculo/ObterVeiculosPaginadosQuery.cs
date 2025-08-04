using Application.Common;
using Application.Queries.Base;
using Application.Queries.Veiculo.Models;

namespace Application.Queries.Veiculo;

public class ObterVeiculosPaginadosQuery : QueryPaginadoBase<VeiculoDto>
{
    public ObterVeiculosPaginadosQuery()
    {
    }

    public ObterVeiculosPaginadosQuery(
        List<FiltroGrid> filtros,
        int paginaAtual = 1,
        int tamanhoPagina = 10,
        string campoOrdenacao = "Id",
        bool descendente = false) : base(filtros, paginaAtual, tamanhoPagina, campoOrdenacao, descendente)
    {
    }
} 

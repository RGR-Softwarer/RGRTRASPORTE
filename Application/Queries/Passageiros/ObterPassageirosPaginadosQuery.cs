using Application.Common;
using Application.Queries.Base;
using Application.Queries.Passageiros.Models;

namespace Application.Queries.Passageiros;

public class ObterPassageirosPaginadosQuery : QueryPaginadoBase<PassageiroDto>
{
    public ObterPassageirosPaginadosQuery()
    {
    }

    public ObterPassageirosPaginadosQuery(
        List<FiltroGrid> filtros,
        int paginaAtual = 1,
        int tamanhoPagina = 10,
        string campoOrdenacao = "Id",
        bool descendente = false) : base(filtros, paginaAtual, tamanhoPagina, campoOrdenacao, descendente)
    {
    }
}
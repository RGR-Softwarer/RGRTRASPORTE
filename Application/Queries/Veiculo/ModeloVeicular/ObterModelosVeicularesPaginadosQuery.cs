using Application.Common;
using Application.Queries.Base;
using Application.Queries.Veiculo.ModeloVeicular.Models;

namespace Application.Queries.Veiculo.ModeloVeicular;

public class ObterModelosVeicularesPaginadosQuery : BaseQuery<BaseResponse<GridModeloVeicularResult>>
{
    public List<FiltroGrid> Filtros { get; set; } = new();
    public int PaginaAtual { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 10;
    public string CampoOrdenacao { get; set; } = "Id";
    public bool Descendente { get; set; } = false;

    public ObterModelosVeicularesPaginadosQuery()
    {
    }

    public ObterModelosVeicularesPaginadosQuery(
        List<FiltroGrid> filtros,
        int paginaAtual = 1,
        int tamanhoPagina = 10,
        string campoOrdenacao = "Id",
        bool descendente = false)
    {
        Filtros = filtros ?? new List<FiltroGrid>();
        PaginaAtual = paginaAtual;
        TamanhoPagina = tamanhoPagina;
        CampoOrdenacao = campoOrdenacao;
        Descendente = descendente;
    }
}

public class GridModeloVeicularResult
{
    public IEnumerable<ModeloVeicularDto> Items { get; set; } = new List<ModeloVeicularDto>();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
} 

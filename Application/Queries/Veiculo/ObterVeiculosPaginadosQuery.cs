using Application.Common;
using Application.Queries.Base;
using Application.Queries.Veiculo.Models;
using Dominio.Enums.Veiculo;

namespace Application.Queries.Veiculo;

public class ObterVeiculosPaginadosQuery : BaseQuery<BaseResponse<GridVeiculoResult>>
{
    public List<FiltroGrid> Filtros { get; set; } = new();
    public int PaginaAtual { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 10;
    public string CampoOrdenacao { get; set; } = "Id";
    public bool Descendente { get; set; } = false;

    public ObterVeiculosPaginadosQuery()
    {
    }

    public ObterVeiculosPaginadosQuery(
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

public class GridVeiculoResult
{
    public IEnumerable<VeiculoDto> Items { get; set; } = new List<VeiculoDto>();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
} 
using Application.Common;
using Application.Queries.Base;
using Application.Queries.Veiculo.Models;

namespace Application.Queries.Veiculo;

public class ListarVeiculosQuery : BaseQuery<BaseResponse<IEnumerable<VeiculoDto>>>
{
    public string PlacaFiltro { get; private set; }
    public string ModeloFiltro { get; private set; }
    public bool? AtivoFiltro { get; private set; }
    public int Pagina { get; private set; }
    public int TamanhoPagina { get; private set; }

    public ListarVeiculosQuery(
        string placaFiltro = null,
        string modeloFiltro = null,
        bool? ativoFiltro = null,
        int pagina = 1,
        int tamanhoPagina = 10)
    {
        PlacaFiltro = placaFiltro;
        ModeloFiltro = modeloFiltro;
        AtivoFiltro = ativoFiltro;
        Pagina = pagina < 1 ? 1 : pagina;
        TamanhoPagina = tamanhoPagina < 1 ? 10 : tamanhoPagina;
    }
} 
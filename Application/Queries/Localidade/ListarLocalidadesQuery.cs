using Application.Common;
using Application.Queries.Base;
using Application.Queries.Localidade.Models;

namespace Application.Queries.Localidade;

public class ListarLocalidadesQuery : BaseQuery<BaseResponse<IEnumerable<LocalidadeDto>>>
{
    public string NomeFiltro { get; private set; }
    public string CidadeFiltro { get; private set; }
    public string EstadoFiltro { get; private set; }
    public bool? AtivoFiltro { get; private set; }
    public int Pagina { get; private set; }
    public int TamanhoPagina { get; private set; }

    public ListarLocalidadesQuery(
        string nomeFiltro = null,
        string cidadeFiltro = null,
        string estadoFiltro = null,
        bool? ativoFiltro = null,
        int pagina = 1,
        int tamanhoPagina = 10)
    {
        NomeFiltro = nomeFiltro;
        CidadeFiltro = cidadeFiltro;
        EstadoFiltro = estadoFiltro;
        AtivoFiltro = ativoFiltro;
        Pagina = pagina < 1 ? 1 : pagina;
        TamanhoPagina = tamanhoPagina < 1 ? 10 : tamanhoPagina;
    }
} 

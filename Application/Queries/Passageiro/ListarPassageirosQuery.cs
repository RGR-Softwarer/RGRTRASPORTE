using Application.Common;
using Application.Queries.Base;
using Application.Queries.Passageiro.Models;

namespace Application.Queries.Passageiro;

public class ListarPassageirosQuery : BaseQuery<BaseResponse<IEnumerable<PassageiroDto>>>
{
    public string NomeFiltro { get; private set; }
    public string DocumentoFiltro { get; private set; }
    public bool? AtivoFiltro { get; private set; }
    public int Pagina { get; private set; }
    public int TamanhoPagina { get; private set; }

    public ListarPassageirosQuery(
        string nomeFiltro = null,
        string documentoFiltro = null,
        bool? ativoFiltro = null,
        int pagina = 1,
        int tamanhoPagina = 10)
    {
        NomeFiltro = nomeFiltro;
        DocumentoFiltro = documentoFiltro;
        AtivoFiltro = ativoFiltro;
        Pagina = pagina < 1 ? 1 : pagina;
        TamanhoPagina = tamanhoPagina < 1 ? 10 : tamanhoPagina;
    }
} 
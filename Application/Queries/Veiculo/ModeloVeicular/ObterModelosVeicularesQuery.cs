using Application.Common;
using Application.Queries.Base;
using Application.Queries.Veiculo.ModeloVeicular.Models;
using Dominio.Enums.Veiculo;

namespace Application.Queries.Veiculo.ModeloVeicular;

public class ObterModelosVeicularesQuery : BaseQuery<BaseResponse<IEnumerable<ModeloVeicularDto>>>
{
    public string? DescricaoFiltro { get; set; }
    public TipoModeloVeiculoEnum? TipoFiltro { get; set; }
    public bool? AtivoFiltro { get; set; }
    public int Pagina { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 10;

    public ObterModelosVeicularesQuery()
    {
    }

    public ObterModelosVeicularesQuery(
        string? descricaoFiltro = null,
        TipoModeloVeiculoEnum? tipoFiltro = null,
        bool? ativoFiltro = null,
        int pagina = 1,
        int tamanhoPagina = 10)
    {
        DescricaoFiltro = descricaoFiltro;
        TipoFiltro = tipoFiltro;
        AtivoFiltro = ativoFiltro;
        Pagina = pagina < 1 ? 1 : pagina;
        TamanhoPagina = tamanhoPagina < 1 ? 10 : tamanhoPagina;
    }
} 
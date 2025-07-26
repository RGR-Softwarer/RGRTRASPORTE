using Dominio.Entidades.Viagens;

namespace Dominio.Interfaces.Infra.Data.Viagens
{
    public interface IViagemPosicaoRepository : IGenericRepository<ViagemPosicao>
    {
        Task<ViagemPosicao> ObterPosicaoCompletaPorIdAsync(long id);
        Task<IEnumerable<ViagemPosicao>> ObterPosicoesPorViagemAsync(long viagemId);
        Task<IEnumerable<ViagemPosicao>> ObterPosicoesPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<ViagemPosicao> ObterUltimaPosicaoPorViagemAsync(long viagemId);
        Task<decimal> CalcularDistanciaTotalPorViagemAsync(long viagemId);
        Task<decimal> CalcularTempoTotalPorViagemAsync(long viagemId);
    }
}

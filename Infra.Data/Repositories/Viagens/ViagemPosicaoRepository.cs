using Microsoft.EntityFrameworkCore;
using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories.Viagens
{
    public class ViagemPosicaoRepository : GenericRepository<ViagemPosicao>, IViagemPosicaoRepository
    {
        public ViagemPosicaoRepository(TransportadorContext context) : base(context) { }

        public async Task<ViagemPosicao> ObterPosicaoCompletaPorIdAsync(long id)
        {
            return await Query()
                .Include(p => p.Viagem)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<ViagemPosicao>> ObterPosicoesPorViagemAsync(long viagemId)
        {
            return await Query()
                .Include(p => p.Viagem)
                .Where(p => p.ViagemId == viagemId)
                .OrderBy(p => p.DataHora)
                .ToListAsync();
        }

        public async Task<IEnumerable<ViagemPosicao>> ObterPosicoesPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(p => p.Viagem)
                .Where(p => p.DataHora >= dataInicio && p.DataHora <= dataFim)
                .OrderBy(p => p.DataHora)
                .ToListAsync();
        }

        public async Task<ViagemPosicao> ObterUltimaPosicaoPorViagemAsync(long viagemId)
        {
            return await Query()
                .Include(p => p.Viagem)
                .Where(p => p.ViagemId == viagemId)
                .OrderByDescending(p => p.DataHora)
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> CalcularDistanciaTotalPorViagemAsync(long viagemId)
        {
            var posicoes = await Query()
                .Where(p => p.ViagemId == viagemId)
                .OrderBy(p => p.DataHora)
                .ToListAsync();

            decimal distanciaTotal = 0;
            for (int i = 1; i < posicoes.Count; i++)
            {
                distanciaTotal += posicoes[i].CalcularDistancia(posicoes[i - 1]);
            }

            return distanciaTotal;
        }

        public async Task<decimal> CalcularTempoTotalPorViagemAsync(long viagemId)
        {
            var posicoes = await Query()
                .Where(p => p.ViagemId == viagemId)
                .OrderBy(p => p.DataHora)
                .ToListAsync();

            if (posicoes.Count < 2)
                return 0;

            var tempoTotal = (posicoes.Last().DataHora - posicoes.First().DataHora).TotalHours;
            return (decimal)tempoTotal;
        }
    }
}
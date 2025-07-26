using Microsoft.EntityFrameworkCore;
using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Interfaces.Infra.Data.Viagens.Gatilho;
using Dominio.Enums.Data;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories.Viagens.Gatilho
{
    public class GatilhoViagemRepository : GenericRepository<GatilhoViagem>, IGatilhoViagemRepository
    {
        public GatilhoViagemRepository(TransportadorContext context) : base(context) { }

        public async Task<GatilhoViagem> ObterGatilhoCompletoPorIdAsync(long id)
        {
            return await Query()
                .Include(g => g.Veiculo)
                .Include(g => g.Viagens)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorVeiculoAsync(long veiculoId)
        {
            return await Query()
                .Where(g => g.VeiculoId == veiculoId)
                .Include(g => g.Veiculo)
                .ToListAsync();
        }

        public async Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorMotoristaAsync(long motoristaId)
        {
            return await Query()
                .Where(g => g.MotoristaId == motoristaId)
                .Include(g => g.Veiculo)
                .ToListAsync();
        }

        public async Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorLocalidadeAsync(long localidadeId, bool origem = true)
        {
            var query = Query();
            
            if (origem)
                query = query.Where(g => g.LocalidadeOrigemId == localidadeId);
            else
                query = query.Where(g => g.LocalidadeDestinoId == localidadeId);

            return await query
                .Include(g => g.Veiculo)
                .ToListAsync();
        }

        public async Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorDiaSemanaAsync(DiaSemanaEnum diaSemana)
        {
            return await Query()
                .Where(g => g.DiasSemana.Contains(diaSemana))
                .Include(g => g.Veiculo)
                .ToListAsync();
        }

        public async Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorHorarioAsync(TimeSpan horarioInicio, TimeSpan horarioFim)
        {
            return await Query()
                .Where(g => g.HorarioSaida >= horarioInicio && g.HorarioChegada <= horarioFim)
                .Include(g => g.Veiculo)
                .ToListAsync();
        }

        public async Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorValorPassagemAsync(decimal valorMinimo, decimal valorMaximo)
        {
            return await Query()
                .Where(g => g.ValorPassagem >= valorMinimo && g.ValorPassagem <= valorMaximo)
                .Include(g => g.Veiculo)
                .ToListAsync();
        }

        public async Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorQuantidadeVagasAsync(int quantidadeMinima, int quantidadeMaxima)
        {
            return await Query()
                .Where(g => g.QuantidadeVagas >= quantidadeMinima && g.QuantidadeVagas <= quantidadeMaxima)
                .Include(g => g.Veiculo)
                .ToListAsync();
        }

        public async Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorDistanciaAsync(decimal distanciaMinima, decimal distanciaMaxima)
        {
            return await Query()
                .Where(g => g.Distancia >= distanciaMinima && g.Distancia <= distanciaMaxima)
                .Include(g => g.Veiculo)
                .ToListAsync();
        }

        public async Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorAtivoAsync(bool ativo)
        {
            return await Query()
                .Where(g => g.Ativo == ativo)
                .Include(g => g.Veiculo)
                .ToListAsync();
        }

        public async Task<bool> ExisteGatilhoConflitanteAsync(TimeSpan horarioSaida, TimeSpan horarioChegada, long veiculoId, long? gatilhoIdExcluir = null)
        {
            var query = Query()
                .Where(g => g.VeiculoId == veiculoId);

            if (gatilhoIdExcluir.HasValue)
                query = query.Where(g => g.Id != gatilhoIdExcluir.Value);

            return await query.AnyAsync(g =>
                (horarioSaida >= g.HorarioSaida && horarioSaida < g.HorarioChegada) ||
                (horarioChegada > g.HorarioSaida && horarioChegada <= g.HorarioChegada) ||
                (horarioSaida <= g.HorarioSaida && horarioChegada >= g.HorarioChegada));
        }

        public async Task<bool> ExisteGatilhoAgendadoParaVeiculo(long veiculoId, DiaSemanaEnum diaSemana)
        {
            return await Query()
                .AnyAsync(g => g.VeiculoId == veiculoId && 
                              g.DiasSemana.Contains(diaSemana) && 
                              g.Ativo);
        }

        public async Task<bool> ExisteGatilhoAgendadoParaMotorista(long motoristaId, DiaSemanaEnum diaSemana)
        {
            return await Query()
                .AnyAsync(g => g.MotoristaId == motoristaId && 
                              g.DiasSemana.Contains(diaSemana) && 
                              g.Ativo);
        }
    }
}
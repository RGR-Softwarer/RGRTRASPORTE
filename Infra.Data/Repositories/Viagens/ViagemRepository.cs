using Dominio.Entidades.Viagens;
using Dominio.Enums.Viagens;
using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Infra.Data.Viagens;
using Infra.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories.Viagens
{
    public class ViagemRepository : GenericRepository<Viagem>, IViagemRepository
    {
        public ViagemRepository(IUnitOfWorkContext unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<Viagem?> ObterPorIdComIncludes(long id)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Include(v => v.GatilhoViagem)
                .Include(v => v.Passageiros)
                    .ThenInclude(p => p.Passageiro)
                .Include(v => v.Posicoes)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorMotoristaAsync(long motoristaId)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Where(v => v.MotoristaId == motoristaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorVeiculoAsync(long veiculoId)
        {
            return await Query()
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Where(v => v.VeiculoId == veiculoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorSituacaoAsync(SituacaoViagemEnum situacao)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Where(v => v.Situacao == situacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorDataAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.Periodo.Data >= dataInicio && v.Periodo.Data <= dataFim)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorLocalidadeAsync(long localidadeId, bool origem = true)
        {
            var query = Query();

            if (origem)
                query = query.Where(v => v.LocalidadeOrigemId == localidadeId);
            else
                query = query.Where(v => v.LocalidadeDestinoId == localidadeId);

            return await query
                .Include(v => v.Veiculo)
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .ToListAsync();
        }

        public async Task<bool> ExisteViagemConflitanteAsync(long veiculoId, DateTime data)
        {
            var existe = await Query()
                .AnyAsync(v => v.Periodo.Data == data.Date && v.VeiculoId == veiculoId);
            
            return existe;
        }

        public async Task<bool> ExisteViagemMotoristaAsync(long motoristaId, DateTime data)
        {
            return await Query()
                .AnyAsync(v =>
                    v.Periodo.Data == data.Date &&
                    v.MotoristaId == motoristaId &&
                    v.Situacao != SituacaoViagemEnum.Cancelada);
        }

        public async Task<bool> ExisteViagemVeiculoAsync(long veiculoId, DateTime data)
        {
            return await Query()
                .AnyAsync(v =>
                    v.Periodo.Data == data.Date &&
                    v.VeiculoId == veiculoId &&
                    v.Situacao != SituacaoViagemEnum.Cancelada);
        }

        public async Task<IEnumerable<Viagem>> ObterViagensAtivasAsync()
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Where(v => v.Ativo && v.Situacao != SituacaoViagemEnum.Cancelada)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensComVagasAsync()
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Where(v => v.VagasDisponiveis > 0 && v.Situacao == SituacaoViagemEnum.Agendada)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensLotadasAsync()
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Where(v => v.Lotado && v.Situacao == SituacaoViagemEnum.Agendada)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensEmAndamentoAsync()
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Include(v => v.Posicoes)
                .Where(v => v.Situacao == SituacaoViagemEnum.EmAndamento)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensFinalizadasAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Where(v => v.Periodo.Data >= dataInicio && v.Periodo.Data <= dataFim)
                .Where(v => v.Situacao == SituacaoViagemEnum.Finalizada)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorVeiculoEPeriodoAsync(long veiculoId, DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Where(v => v.VeiculoId == veiculoId && v.Periodo.Data >= dataInicio && v.Periodo.Data <= dataFim)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorMotoristaEPeriodoAsync(long motoristaId, DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Where(v => v.MotoristaId == motoristaId && v.Periodo.Data >= dataInicio && v.Periodo.Data <= dataFim)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensAgendadasAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Where(v => v.Periodo.Data >= dataInicio && v.Periodo.Data <= dataFim)
                .Where(v => v.Situacao == SituacaoViagemEnum.Agendada)
                .ToListAsync();
        }

        // REMOVIDO: ObterViagensPorValorPassagemAsync - ValorPassagem não existe mais na entidade Viagem

        public async Task<int> ContarViagensAtivas()
        {
            return await Query()
                .CountAsync(v => v.Ativo && v.Situacao != SituacaoViagemEnum.Cancelada);
        }

        public async Task<int> ContarViagensComVagas()
        {
            return await Query()
                .CountAsync(v => v.VagasDisponiveis > 0 && v.Situacao == SituacaoViagemEnum.Agendada);
        }

        public async Task<int> ContarViagensEmAndamento()
        {
            return await Query()
                .CountAsync(v => v.Situacao == SituacaoViagemEnum.EmAndamento);
        }

        public async Task<int> ContarViagensFinalizadas()
        {
            return await Query()
                .CountAsync(v => v.Situacao == SituacaoViagemEnum.Finalizada);
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorRota(long origemId, long destinoId)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Where(v => v.LocalidadeOrigemId == origemId && v.LocalidadeDestinoId == destinoId)
                .Where(v => v.Situacao == SituacaoViagemEnum.Agendada)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorGatilhoAsync(long gatilhoId, DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Where(v => v.GatilhoViagemId == gatilhoId && v.Periodo.Data >= dataInicio && v.Periodo.Data <= dataFim)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPassageiroAsync(long passageiroId, DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Include(v => v.Passageiros)
                .Where(v =>
                    v.Passageiros.Any(p => p.PassageiroId == passageiroId) &&
                    v.Periodo.Data >= dataInicio && v.Periodo.Data <= dataFim)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorSituacaoEPeriodoAsync(SituacaoViagemEnum situacao, DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Where(v => v.Situacao == situacao && v.Periodo.Data >= dataInicio && v.Periodo.Data <= dataFim)
                .ToListAsync();
        }

        public async Task<bool> ExisteViagemNoPeriodoAsync(long veiculoId, DateTime data, TimeSpan horarioInicio, TimeSpan horarioFim)
        {
            return await Query()
                .AnyAsync(v =>
                    v.VeiculoId == veiculoId &&
                    v.Periodo.Data == data.Date &&
                    ((v.Periodo.HoraSaida >= horarioInicio && v.Periodo.HoraSaida <= horarioFim) ||
                     (v.Periodo.HoraChegada >= horarioInicio && v.Periodo.HoraChegada <= horarioFim) ||
                     (v.Periodo.HoraSaida <= horarioInicio && v.Periodo.HoraChegada >= horarioFim)));
        }

        public async Task<IEnumerable<Viagem>> ObterViagensProximasAsync(DateTime data, TimeSpan horario, int minutos = 30)
        {
            var horarioLimite = horario.Add(TimeSpan.FromMinutes(minutos));
            
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.Motorista)
                .Include(v => v.LocalidadeOrigem)
                .Include(v => v.LocalidadeDestino)
                .Where(v =>
                    v.Periodo.Data == data.Date &&
                    v.Periodo.HoraSaida >= horario &&
                    v.Periodo.HoraSaida <= horarioLimite &&
                    v.Situacao == SituacaoViagemEnum.Agendada)
                .OrderBy(v => v.Periodo.HoraSaida)
                .ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Dominio.Entidades.Viagens;
using Dominio.Enums.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using Dominio.Interfaces.Infra.Data.Veiculo;
using Dominio.Interfaces.Infra.Data.Localidades;
using Dominio.Interfaces.Infra.Data.Motorista;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories.Viagens
{
    public class ViagemRepository : GenericRepository<Viagem>, IViagemRepository
    {
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly ILocalidadeRepository _localidadeRepository;
        private readonly IMotoristaRepository _motoristaRepository;

        public ViagemRepository(
            TransportadorContext context, 
            IVeiculoRepository veiculoRepository,
            ILocalidadeRepository localidadeRepository,
            IMotoristaRepository motoristaRepository) : base(context)
        {
            _veiculoRepository = veiculoRepository;
            _localidadeRepository = localidadeRepository;
            _motoristaRepository = motoristaRepository;
        }

        public async Task<Viagem> ObterViagemCompletaPorIdAsync(long id)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Include(v => v.Passageiros)
                .Include(v => v.Posicoes)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorSituacaoAsync(SituacaoViagemEnum situacao)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.Situacao == situacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorDataAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.DataViagem >= dataInicio && v.DataViagem <= dataFim)
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
                .Include(v => v.GatilhoViagem)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorVeiculoAsync(long veiculoId)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.VeiculoId == veiculoId)
                .ToListAsync();
        }

        public async Task<bool> ExisteViagemConflitanteAsync(DateTime data, TimeSpan horaSaida, TimeSpan horaChegada, long veiculoId, long? viagemIdExcluir = null)
        {
            var query = Query()
                .Where(v => v.DataViagem == data.Date && v.VeiculoId == veiculoId);

            if (viagemIdExcluir.HasValue)
                query = query.Where(v => v.Id != viagemIdExcluir.Value);

            return await query.AnyAsync(v =>
                (horaSaida >= v.HorarioSaida && horaSaida < v.HorarioChegada) ||
                (horaChegada > v.HorarioSaida && horaChegada <= v.HorarioChegada) ||
                (horaSaida <= v.HorarioSaida && horaChegada >= v.HorarioChegada));
        }

        public async Task<bool> ExisteViagemAgendadaParaVeiculo(long veiculoId, DateTime data)
        {
            return await Query()
                .AnyAsync(v => v.VeiculoId == veiculoId && 
                              v.DataViagem == data.Date && 
                              v.Situacao == SituacaoViagemEnum.Agendada);
        }

        public async Task<bool> ExisteViagemAgendadaParaMotorista(long motoristaId, DateTime data)
        {
            return await Query()
                .AnyAsync(v => v.MotoristaId == motoristaId && 
                              v.DataViagem == data.Date && 
                              v.Situacao == SituacaoViagemEnum.Agendada);
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorMotoristaAsync(long motoristaId)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.MotoristaId == motoristaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorPassageiroAsync(long passageiroId)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Include(v => v.Passageiros)
                .Where(v => v.Passageiros.Any(p => p.PassageiroId == passageiroId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorGatilhoAsync(long gatilhoId)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.GatilhoViagemId == gatilhoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensEmAndamentoAsync()
        {
            return await ObterViagensPorSituacaoAsync(SituacaoViagemEnum.EmAndamento);
        }

        public async Task<IEnumerable<Viagem>> ObterViagensAgendadasAsync()
        {
            return await ObterViagensPorSituacaoAsync(SituacaoViagemEnum.Agendada);
        }

        public async Task<IEnumerable<Viagem>> ObterViagensFinalizadasAsync()
        {
            return await ObterViagensPorSituacaoAsync(SituacaoViagemEnum.Finalizada);
        }

        public async Task<IEnumerable<Viagem>> ObterViagensCanceladasAsync()
        {
            return await ObterViagensPorSituacaoAsync(SituacaoViagemEnum.Cancelada);
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, SituacaoViagemEnum? situacao = null)
        {
            var query = Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.DataViagem >= dataInicio && v.DataViagem <= dataFim);

            if (situacao.HasValue)
                query = query.Where(v => v.Situacao == situacao.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorVeiculoEPeriodoAsync(long veiculoId, DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.VeiculoId == veiculoId && v.DataViagem >= dataInicio && v.DataViagem <= dataFim)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorMotoristaEPeriodoAsync(long motoristaId, DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.MotoristaId == motoristaId && v.DataViagem >= dataInicio && v.DataViagem <= dataFim)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorLocalidadeEPeriodoAsync(long localidadeId, bool origem, DateTime dataInicio, DateTime dataFim)
        {
            var query = Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.DataViagem >= dataInicio && v.DataViagem <= dataFim);

            if (origem)
                query = query.Where(v => v.LocalidadeOrigemId == localidadeId);
            else
                query = query.Where(v => v.LocalidadeDestinoId == localidadeId);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorValorPassagemAsync(decimal valorMinimo, decimal valorMaximo)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.ValorPassagem >= valorMinimo && v.ValorPassagem <= valorMaximo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorDistanciaAsync(decimal distanciaMinima, decimal distanciaMaxima)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.Distancia >= distanciaMinima && v.Distancia <= distanciaMaxima)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorQuantidadeVagasAsync(int quantidadeMinima, int quantidadeMaxima)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.QuantidadeVagas >= quantidadeMinima && v.QuantidadeVagas <= quantidadeMaxima)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorVagasDisponiveisAsync(int quantidadeMinima, int quantidadeMaxima)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.VagasDisponiveis >= quantidadeMinima && v.VagasDisponiveis <= quantidadeMaxima)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorLotacaoAsync(bool lotado)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.Lotado == lotado)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorAtivoAsync(bool ativo)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.Ativo == ativo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorGatilhoEPeriodoAsync(long gatilhoId, DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.GatilhoViagemId == gatilhoId && v.DataViagem >= dataInicio && v.DataViagem <= dataFim)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorPassageiroEPeriodoAsync(long passageiroId, DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Include(v => v.Passageiros)
                .Where(v => v.Passageiros.Any(p => p.PassageiroId == passageiroId) && 
                           v.DataViagem >= dataInicio && v.DataViagem <= dataFim)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorSituacaoEPeriodoAsync(SituacaoViagemEnum situacao, DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.Situacao == situacao && v.DataViagem >= dataInicio && v.DataViagem <= dataFim)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorSituacaoEVeiculoAsync(SituacaoViagemEnum situacao, long veiculoId)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.Situacao == situacao && v.VeiculoId == veiculoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorSituacaoEMotoristaAsync(SituacaoViagemEnum situacao, long motoristaId)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.Situacao == situacao && v.MotoristaId == motoristaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorSituacaoELocalidadeAsync(SituacaoViagemEnum situacao, long localidadeId, bool origem)
        {
            var query = Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.Situacao == situacao);

            if (origem)
                query = query.Where(v => v.LocalidadeOrigemId == localidadeId);
            else
                query = query.Where(v => v.LocalidadeDestinoId == localidadeId);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorSituacaoEGatilhoAsync(SituacaoViagemEnum situacao, long gatilhoId)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Where(v => v.Situacao == situacao && v.GatilhoViagemId == gatilhoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorSituacaoEPassageiroAsync(SituacaoViagemEnum situacao, long passageiroId)
        {
            return await Query()
                .Include(v => v.Veiculo)
                .Include(v => v.GatilhoViagem)
                .Include(v => v.Passageiros)
                .Where(v => v.Situacao == situacao && v.Passageiros.Any(p => p.PassageiroId == passageiroId))
                .ToListAsync();
        }
    }
}
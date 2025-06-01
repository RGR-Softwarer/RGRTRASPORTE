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
            var viagem = await Query()
                .Include(v => v.GatilhoViagem)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (viagem != null)
            {
                var veiculo = await _veiculoRepository.ObterPorIdAsync(viagem.VeiculoId);
                var localidadeOrigem = await _localidadeRepository.ObterPorIdAsync(viagem.LocalidadeOrigemId);
                var localidadeDestino = await _localidadeRepository.ObterPorIdAsync(viagem.LocalidadeDestinoId);
                var motorista = await _motoristaRepository.ObterPorIdAsync(viagem.MotoristaId);

                if (veiculo != null)
                {
                    viagem.GetType().GetProperty("Veiculo")?.SetValue(viagem, veiculo);
                }

                if (localidadeOrigem != null)
                {
                    viagem.GetType().GetProperty("LocalidadeOrigem")?.SetValue(viagem, localidadeOrigem);
                }

                if (localidadeDestino != null)
                {
                    viagem.GetType().GetProperty("LocalidadeDestino")?.SetValue(viagem, localidadeDestino);
                }

                if (motorista != null)
                {
                    viagem.GetType().GetProperty("Motorista")?.SetValue(viagem, motorista);
                }
            }

            return viagem;
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorSituacaoAsync(SituacaoViagemEnum situacao)
        {
            var viagens = await Query()
                .Where(v => v.Situacao == situacao)
                .ToListAsync();

            foreach (var viagem in viagens)
            {
                var veiculo = await _veiculoRepository.ObterPorIdAsync(viagem.VeiculoId);
                var localidadeOrigem = await _localidadeRepository.ObterPorIdAsync(viagem.LocalidadeOrigemId);
                var localidadeDestino = await _localidadeRepository.ObterPorIdAsync(viagem.LocalidadeDestinoId);
                var motorista = await _motoristaRepository.ObterPorIdAsync(viagem.MotoristaId);

                if (veiculo != null)
                {
                    viagem.GetType().GetProperty("Veiculo")?.SetValue(viagem, veiculo);
                }

                if (localidadeOrigem != null)
                {
                    viagem.GetType().GetProperty("LocalidadeOrigem")?.SetValue(viagem, localidadeOrigem);
                }

                if (localidadeDestino != null)
                {
                    viagem.GetType().GetProperty("LocalidadeDestino")?.SetValue(viagem, localidadeDestino);
                }

                if (motorista != null)
                {
                    viagem.GetType().GetProperty("Motorista")?.SetValue(viagem, motorista);
                }
            }

            return viagens;
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorDataAsync(DateTime dataInicio, DateTime dataFim)
        {
            var viagens = await Query()
                .Where(v => v.DataViagem >= dataInicio && v.DataViagem <= dataFim)
                .OrderBy(v => v.DataViagem)
                .ToListAsync();

            foreach (var viagem in viagens)
            {
                viagem.GetType().GetProperty("Veiculo")?.SetValue(viagem, 
                    await _veiculoRepository.ObterPorIdAsync(viagem.VeiculoId));

                viagem.GetType().GetProperty("LocalidadeOrigem")?.SetValue(viagem,
                    await _localidadeRepository.ObterPorIdAsync(viagem.LocalidadeOrigemId));

                viagem.GetType().GetProperty("LocalidadeDestino")?.SetValue(viagem,
                    await _localidadeRepository.ObterPorIdAsync(viagem.LocalidadeDestinoId));

                viagem.GetType().GetProperty("Motorista")?.SetValue(viagem,
                    await _motoristaRepository.ObterPorIdAsync(viagem.MotoristaId));
            }

            return viagens;
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorLocalidadeAsync(long localidadeId, bool origem = true)
        {
            var query = Query().AsQueryable();

            if (origem)
                query = query.Where(v => v.LocalidadeOrigemId == localidadeId);
            else
                query = query.Where(v => v.LocalidadeDestinoId == localidadeId);

            var viagens = await query
                .OrderBy(v => v.DataViagem)
                .ToListAsync();

            foreach (var viagem in viagens)
            {
                viagem.GetType().GetProperty("Veiculo")?.SetValue(viagem,
                    await _veiculoRepository.ObterPorIdAsync(viagem.VeiculoId));

                viagem.GetType().GetProperty("LocalidadeOrigem")?.SetValue(viagem,
                    await _localidadeRepository.ObterPorIdAsync(viagem.LocalidadeOrigemId));

                viagem.GetType().GetProperty("LocalidadeDestino")?.SetValue(viagem,
                    await _localidadeRepository.ObterPorIdAsync(viagem.LocalidadeDestinoId));

                viagem.GetType().GetProperty("Motorista")?.SetValue(viagem,
                    await _motoristaRepository.ObterPorIdAsync(viagem.MotoristaId));
            }

            return viagens;
        }

        public async Task<IEnumerable<Viagem>> ObterViagensPorVeiculoAsync(long veiculoId)
        {
            var viagens = await Query()
                .Where(v => v.VeiculoId == veiculoId)
                .OrderBy(v => v.DataViagem)
                .ToListAsync();

            foreach (var viagem in viagens)
            {
                viagem.GetType().GetProperty("LocalidadeOrigem")?.SetValue(viagem,
                    await _localidadeRepository.ObterPorIdAsync(viagem.LocalidadeOrigemId));

                viagem.GetType().GetProperty("LocalidadeDestino")?.SetValue(viagem,
                    await _localidadeRepository.ObterPorIdAsync(viagem.LocalidadeDestinoId));

                viagem.GetType().GetProperty("Motorista")?.SetValue(viagem,
                    await _motoristaRepository.ObterPorIdAsync(viagem.MotoristaId));
            }

            return viagens;
        }

        public async Task<bool> ExisteViagemConflitanteAsync(
            DateTime data,
            TimeSpan horarioSaida,
            TimeSpan horarioChegada,
            long veiculoId,
            long? viagemIdExcluir = null)
        {
            var query = Query()
                .Where(v => v.DataViagem.Date == data.Date
                    && v.VeiculoId == veiculoId
                    && v.Situacao == SituacaoViagemEnum.Agendada);

            if (viagemIdExcluir.HasValue)
                query = query.Where(v => v.Id != viagemIdExcluir.Value);

            var viagensExistentes = await query.ToListAsync();

            return viagensExistentes.Any(v =>
                (horarioSaida >= v.HorarioSaida && horarioSaida <= v.HorarioChegada) ||
                (horarioChegada >= v.HorarioSaida && horarioChegada <= v.HorarioChegada) ||
                (horarioSaida <= v.HorarioSaida && horarioChegada >= v.HorarioChegada));
        }

        public async Task<(IEnumerable<Viagem> Items, int Total)> ObterViagensFiltradas(
            int paginaAtual,
            int tamanhoPagina,
            string campoOrdenacao,
            bool descendente,
            DateTime? dataInicio = null,
            DateTime? dataFim = null,
            long? localidadeOrigemId = null,
            long? localidadeDestinoId = null,
            long? veiculoId = null,
            SituacaoViagemEnum? situacao = null,
            bool? ativo = null)
        {
            var query = Query().AsQueryable();

            if (dataInicio.HasValue)
                query = query.Where(v => v.DataViagem >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(v => v.DataViagem <= dataFim.Value);

            if (localidadeOrigemId.HasValue)
                query = query.Where(v => v.LocalidadeOrigemId == localidadeOrigemId.Value);

            if (localidadeDestinoId.HasValue)
                query = query.Where(v => v.LocalidadeDestinoId == localidadeDestinoId.Value);

            if (veiculoId.HasValue)
                query = query.Where(v => v.VeiculoId == veiculoId.Value);

            if (situacao.HasValue)
                query = query.Where(v => v.Situacao == situacao.Value);

            if (ativo.HasValue)
                query = query.Where(v => v.Ativo == ativo.Value);

            var total = await query.CountAsync();

            if (!string.IsNullOrEmpty(campoOrdenacao))
            {
                query = descendente
                    ? query.OrderByDescending(v => EF.Property<object>(v, campoOrdenacao))
                    : query.OrderBy(v => EF.Property<object>(v, campoOrdenacao));
            }
            else
            {
                query = query.OrderByDescending(v => v.DataViagem);
            }

            var items = await query
                .Skip((paginaAtual - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            foreach (var viagem in items)
            {
                viagem.GetType().GetProperty("Veiculo")?.SetValue(viagem,
                    await _veiculoRepository.ObterPorIdAsync(viagem.VeiculoId));

                viagem.GetType().GetProperty("LocalidadeOrigem")?.SetValue(viagem,
                    await _localidadeRepository.ObterPorIdAsync(viagem.LocalidadeOrigemId));

                viagem.GetType().GetProperty("LocalidadeDestino")?.SetValue(viagem,
                    await _localidadeRepository.ObterPorIdAsync(viagem.LocalidadeDestinoId));

                viagem.GetType().GetProperty("Motorista")?.SetValue(viagem,
                    await _motoristaRepository.ObterPorIdAsync(viagem.MotoristaId));
            }

            return (items, total);
        }

        public async Task<IEnumerable<Viagem>> ObterViagensAgendadasPorData(DateTime data)
        {
            var viagens = await Query()
                .Where(v => v.DataViagem.Date == data.Date && 
                           v.Situacao == SituacaoViagemEnum.Agendada)
                .ToListAsync();

            foreach (var viagem in viagens)
            {
                viagem.GetType().GetProperty("Veiculo")?.SetValue(viagem,
                    await _veiculoRepository.ObterPorIdAsync(viagem.VeiculoId));
            }

            return viagens;
        }

        public async Task<IEnumerable<Viagem>> ObterViagensEmAndamentoPorVeiculo(long veiculoId)
        {
            return await Query()
                .Where(v => v.VeiculoId == veiculoId && 
                           v.Situacao == SituacaoViagemEnum.EmAndamento)
                .ToListAsync();
        }

        public async Task<bool> ExisteViagemAgendadaParaVeiculo(long veiculoId, DateTime data)
        {
            return await Query()
                .AnyAsync(v => v.VeiculoId == veiculoId && 
                              v.DataViagem.Date == data.Date && 
                              v.Situacao == SituacaoViagemEnum.Agendada);
        }

        public async Task<bool> ExisteViagemAgendadaParaMotorista(long motoristaId, DateTime data)
        {
            return await Query()
                .AnyAsync(v => v.MotoristaId == motoristaId &&
                              v.DataViagem.Date == data.Date &&
                              v.Situacao == SituacaoViagemEnum.Agendada);
        }
    }
}
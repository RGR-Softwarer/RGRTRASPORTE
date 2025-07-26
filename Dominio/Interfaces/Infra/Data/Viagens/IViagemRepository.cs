using Dominio.Entidades.Viagens;
using Dominio.Enums.Viagens;

namespace Dominio.Interfaces.Infra.Data.Viagens
{
    public interface IViagemRepository : IGenericRepository<Viagem>
    {
        Task<Viagem> ObterViagemCompletaPorIdAsync(long id);
        Task<IEnumerable<Viagem>> ObterViagensPorSituacaoAsync(SituacaoViagemEnum situacao);
        Task<IEnumerable<Viagem>> ObterViagensPorDataAsync(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Viagem>> ObterViagensPorLocalidadeAsync(long localidadeId, bool origem = true);
        Task<IEnumerable<Viagem>> ObterViagensPorVeiculoAsync(long veiculoId);
        Task<bool> ExisteViagemConflitanteAsync(DateTime data, TimeSpan horaSaida, TimeSpan horaChegada, long veiculoId, long? viagemIdExcluir = null);
        Task<bool> ExisteViagemAgendadaParaVeiculo(long veiculoId, DateTime data);
        Task<bool> ExisteViagemAgendadaParaMotorista(long motoristaId, DateTime data);
        Task<IEnumerable<Viagem>> ObterViagensPorMotoristaAsync(long motoristaId);
        Task<IEnumerable<Viagem>> ObterViagensPorPassageiroAsync(long passageiroId);
        Task<IEnumerable<Viagem>> ObterViagensPorGatilhoAsync(long gatilhoId);
        Task<IEnumerable<Viagem>> ObterViagensEmAndamentoAsync();
        Task<IEnumerable<Viagem>> ObterViagensAgendadasAsync();
        Task<IEnumerable<Viagem>> ObterViagensFinalizadasAsync();
        Task<IEnumerable<Viagem>> ObterViagensCanceladasAsync();
        Task<IEnumerable<Viagem>> ObterViagensPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, SituacaoViagemEnum? situacao = null);
        Task<IEnumerable<Viagem>> ObterViagensPorVeiculoEPeriodoAsync(long veiculoId, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Viagem>> ObterViagensPorMotoristaEPeriodoAsync(long motoristaId, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Viagem>> ObterViagensPorLocalidadeEPeriodoAsync(long localidadeId, bool origem, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Viagem>> ObterViagensPorValorPassagemAsync(decimal valorMinimo, decimal valorMaximo);
        Task<IEnumerable<Viagem>> ObterViagensPorDistanciaAsync(decimal distanciaMinima, decimal distanciaMaxima);
        Task<IEnumerable<Viagem>> ObterViagensPorQuantidadeVagasAsync(int quantidadeMinima, int quantidadeMaxima);
        Task<IEnumerable<Viagem>> ObterViagensPorVagasDisponiveisAsync(int quantidadeMinima, int quantidadeMaxima);
        Task<IEnumerable<Viagem>> ObterViagensPorLotacaoAsync(bool lotado);
        Task<IEnumerable<Viagem>> ObterViagensPorAtivoAsync(bool ativo);
        Task<IEnumerable<Viagem>> ObterViagensPorGatilhoEPeriodoAsync(long gatilhoId, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Viagem>> ObterViagensPorPassageiroEPeriodoAsync(long passageiroId, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Viagem>> ObterViagensPorSituacaoEPeriodoAsync(SituacaoViagemEnum situacao, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Viagem>> ObterViagensPorSituacaoEVeiculoAsync(SituacaoViagemEnum situacao, long veiculoId);
        Task<IEnumerable<Viagem>> ObterViagensPorSituacaoEMotoristaAsync(SituacaoViagemEnum situacao, long motoristaId);
        Task<IEnumerable<Viagem>> ObterViagensPorSituacaoELocalidadeAsync(SituacaoViagemEnum situacao, long localidadeId, bool origem);
        Task<IEnumerable<Viagem>> ObterViagensPorSituacaoEGatilhoAsync(SituacaoViagemEnum situacao, long gatilhoId);
        Task<IEnumerable<Viagem>> ObterViagensPorSituacaoEPassageiroAsync(SituacaoViagemEnum situacao, long passageiroId);
    }
}

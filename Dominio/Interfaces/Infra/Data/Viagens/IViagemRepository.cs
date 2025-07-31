using Dominio.Entidades.Viagens;
using Dominio.Enums.Viagens;

namespace Dominio.Interfaces.Infra.Data.Viagens
{
    public interface IViagemRepository : IGenericRepository<Viagem>
    {
        // M�todos de consulta b�sicos
        Task<Viagem?> ObterPorIdComIncludes(long id);
        Task<IEnumerable<Viagem>> ObterViagensPorSituacaoAsync(SituacaoViagemEnum situacao);
        Task<IEnumerable<Viagem>> ObterViagensPorDataAsync(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Viagem>> ObterViagensPorLocalidadeAsync(long localidadeId, bool origem = true);
        Task<IEnumerable<Viagem>> ObterViagensPorMotoristaAsync(long motoristaId);
        Task<IEnumerable<Viagem>> ObterViagensPorVeiculoAsync(long veiculoId);

        // M�todos de verifica��o de exist�ncia
        Task<bool> ExisteViagemConflitanteAsync(long veiculoId, DateTime data);
        Task<bool> ExisteViagemMotoristaAsync(long motoristaId, DateTime data);
        Task<bool> ExisteViagemVeiculoAsync(long veiculoId, DateTime data);
        Task<bool> ExisteViagemNoPeriodoAsync(long veiculoId, DateTime data, TimeSpan horarioInicio, TimeSpan horarioFim);

        // M�todos por estado/situa��o
        Task<IEnumerable<Viagem>> ObterViagensAtivasAsync();
        Task<IEnumerable<Viagem>> ObterViagensComVagasAsync();
        Task<IEnumerable<Viagem>> ObterViagensLotadasAsync();
        Task<IEnumerable<Viagem>> ObterViagensEmAndamentoAsync();
        Task<IEnumerable<Viagem>> ObterViagensFinalizadasAsync(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Viagem>> ObterViagensAgendadasAsync(DateTime dataInicio, DateTime dataFim);

        // M�todos por per�odo
        Task<IEnumerable<Viagem>> ObterViagensPorVeiculoEPeriodoAsync(long veiculoId, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Viagem>> ObterViagensPorMotoristaEPeriodoAsync(long motoristaId, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Viagem>> ObterViagensPorSituacaoEPeriodoAsync(SituacaoViagemEnum situacao, DateTime dataInicio, DateTime dataFim);

        // M�todos especializados
        Task<IEnumerable<Viagem>> ObterViagensPorRota(long origemId, long destinoId);
        Task<IEnumerable<Viagem>> ObterViagensPorGatilhoAsync(long gatilhoId, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Viagem>> ObterViagensPassageiroAsync(long passageiroId, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Viagem>> ObterViagensProximasAsync(DateTime data, TimeSpan horario, int minutos = 30);

        // M�todos de contagem
        Task<int> ContarViagensAtivas();
        Task<int> ContarViagensComVagas();
        Task<int> ContarViagensEmAndamento();
        Task<int> ContarViagensFinalizadas();
    }
}

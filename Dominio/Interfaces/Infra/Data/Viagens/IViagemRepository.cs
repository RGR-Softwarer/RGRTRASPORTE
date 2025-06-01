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
    }
}

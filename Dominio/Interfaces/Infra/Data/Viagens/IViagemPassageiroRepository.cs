using Dominio.Entidades.Viagens;

namespace Dominio.Interfaces.Infra.Data.Viagens
{
    public interface IViagemPassageiroRepository : IGenericRepository<ViagemPassageiro>
    {
        Task<ViagemPassageiro> ObterViagemPassageiroCompletoAsync(long id);
        Task<IEnumerable<ViagemPassageiro>> ObterPassageirosPorViagemAsync(long viagemId);
        Task<IEnumerable<ViagemPassageiro>> ObterViagensPorPassageiroAsync(long passageiroId);
        Task<IEnumerable<ViagemPassageiro>> ObterPassageirosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<ViagemPassageiro>> ObterPassageirosPorConfirmacaoAsync(bool confirmado);
        Task<int> ObterQuantidadePassageirosPorViagemAsync(long viagemId);
        Task<int> ObterQuantidadeViagensPorPassageiroAsync(long passageiroId);
        Task<bool> ExistePassageiroNaViagemAsync(long viagemId, long passageiroId);
        Task<bool> ExisteViagemComPassageiroAsync(long passageiroId);
    }
}
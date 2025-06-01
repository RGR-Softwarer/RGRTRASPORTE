using Dominio.Entidades.Viagens;

namespace Dominio.Interfaces.Infra.Data.Viagens
{
    public interface IViagemPassageiroRepository : IGenericRepository<ViagemPassageiro>
    {
        Task<ViagemPassageiro> ObterViagemPassageiroCompletoAsync(long id);
        Task<IEnumerable<ViagemPassageiro>> ObterPassageirosPorViagemAsync(long viagemId);
        Task<IEnumerable<ViagemPassageiro>> ObterViagensPorPassageiroAsync(long passageiroId);
    }
}
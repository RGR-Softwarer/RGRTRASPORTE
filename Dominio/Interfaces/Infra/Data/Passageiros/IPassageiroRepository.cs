using Dominio.Entidades.Pessoas.Passageiros;

namespace Dominio.Interfaces.Infra.Data.Passageiros
{
    public interface IPassageiroRepository : IGenericRepository<Passageiro>
    {
        Task<Passageiro> ObterPassageiroCompletoAsync(long id);
    }
}
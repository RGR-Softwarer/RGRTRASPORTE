using Dominio.Entidades.Pessoas;

namespace Dominio.Interfaces.Infra.Data.Motorista
{
    public interface IMotoristaRepository : IGenericRepository<Dominio.Entidades.Pessoas.Motorista>
    {
        Task<Dominio.Entidades.Pessoas.Motorista> ObterMotoristaPorCpfAsync(string cpf);
        Task<bool> ExisteMotoristaPorCpfAsync(string cpf, long? idExcluir = null);
        Task<bool> ExisteMotoristaPorCnhAsync(string cnh, long? idExcluir = null);
    }
} 
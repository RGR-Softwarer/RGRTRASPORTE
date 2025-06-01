using Dominio.Entidades.Pessoas.Passageiros;

namespace Dominio.Interfaces.Service.Passageiros;

public interface IPassageiroService
{
    Task<Passageiro> ObterPorIdAsync(long id);
    Task<IEnumerable<Passageiro>> ObterTodosAsync();
    Task<Passageiro> CriarAsync(Passageiro passageiro);
    Task<Passageiro> AtualizarAsync(Passageiro passageiro);
    Task<bool> RemoverAsync(long id);
}
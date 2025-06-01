using Dominio.Entidades.Pessoas;

namespace Dominio.Interfaces.Infra.Data.Pessoas;

public interface IPessoaRepository<T> where T : Pessoa
{
    Task<T> ObterPorIdAsync(long id, CancellationToken cancellationToken);
    Task<T> ObterPorCpfAsync(string cpf, CancellationToken cancellationToken);
    Task<IEnumerable<T>> ObterTodosAsync(CancellationToken cancellationToken);
    Task<IEnumerable<T>> ObterAtivosAsync(CancellationToken cancellationToken);
    Task AdicionarAsync(T pessoa, CancellationToken cancellationToken);
    Task AtualizarAsync(T pessoa, CancellationToken cancellationToken);
    Task RemoverAsync(T pessoa, CancellationToken cancellationToken);
} 
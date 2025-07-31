using Dominio.Entidades.Pessoas;
using Dominio.Interfaces.Infra.Data.Pessoas;
using Microsoft.Extensions.Logging;

namespace Service.Services.Pessoas;

public abstract class PessoaService<T> where T : Pessoa
{
    protected readonly IPessoaRepository<T> _repository;
    protected readonly ILogger<PessoaService<T>> _logger;

    protected PessoaService(
        IPessoaRepository<T> repository,
        ILogger<PessoaService<T>> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public virtual async Task<T> ObterPorIdAsync(long id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Obtendo pessoa por ID {Id}", id);
        return await _repository.ObterPorIdAsync(id, cancellationToken);
    }

    public virtual async Task<T> ObterPorCpfAsync(string cpf, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Obtendo pessoa por CPF {CPF}", cpf);
        return await _repository.ObterPorCpfAsync(cpf, cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> ObterTodosAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Obtendo todas as pessoas");
        return await _repository.ObterTodosAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> ObterAtivosAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Obtendo pessoas ativas");
        return await _repository.ObterAtivosAsync(cancellationToken);
    }

    public virtual async Task AdicionarAsync(T pessoa, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adicionando pessoa {Nome}", pessoa.Nome);
        await _repository.AdicionarAsync(pessoa, cancellationToken);
    }

    public virtual async Task AtualizarAsync(T pessoa, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Atualizando pessoa {Id}", pessoa.Id);
        await _repository.AtualizarAsync(pessoa, cancellationToken);
    }

    public virtual async Task RemoverAsync(T pessoa, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removendo pessoa {Id}", pessoa.Id);
        await _repository.RemoverAsync(pessoa, cancellationToken);
    }
} 

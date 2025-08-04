using Dominio.Enums.Pessoas;
using Dominio.Events.Base;
using Dominio.Exceptions;
using Dominio.Interfaces.Service.Pessoas;
using Dominio.ValueObjects;

namespace Dominio.Entidades.Pessoas;

public abstract class Pessoa : AggregateRoot, IPessoa
{
    protected Pessoa() { } // Construtor protegido para EF Core

    protected Pessoa(
        string nome,
        CPF cpf,
        string telefone,
        string email,
        SexoEnum sexo,
        string observacao)
    {
        ValidarNome(nome);
        ValidarTelefone(telefone);
        ValidarEmail(email);

        Nome = nome;
        CPF = cpf;
        Telefone = telefone;
        Email = email;
        Sexo = sexo;
        Observacao = observacao;
        Situacao = true;
    }

    public string Nome { get; protected set; }
    public CPF CPF { get; protected set; }
    public string Telefone { get; protected set; }
    public string Email { get; protected set; }
    public SexoEnum Sexo { get; protected set; }
    public long LocalidadeId { get; protected set; }
    public long LocalidadeEmbarqueId { get; protected set; }
    public long LocalidadeDesembarqueId { get; protected set; }
    public bool Situacao { get; protected set; }
    public string Observacao { get; protected set; }

    public string CPF_Formatado => CPF.NumeroFormatado;

    public virtual void Atualizar(
        string nome,
        CPF cpf,
        string telefone,
        string email,
        SexoEnum sexo,
        long localidadeId,
        long localidadeEmbarqueId,
        long localidadeDesembarqueId,
        string observacao)
    {
        ValidarNome(nome);
        ValidarTelefone(telefone);
        ValidarEmail(email);

        Nome = nome;
        CPF = cpf;
        Telefone = telefone;
        Email = email;
        Sexo = sexo;
        LocalidadeId = localidadeId;
        LocalidadeEmbarqueId = localidadeEmbarqueId;
        LocalidadeDesembarqueId = localidadeDesembarqueId;
        Observacao = observacao;
        UpdateTimestamp();
    }

    public void Ativar()
    {
        if (Situacao)
            throw new DomainException("Pessoa já está ativa.");

        Situacao = true;
        UpdateTimestamp();
    }

    public void Inativar()
    {
        if (!Situacao)
            throw new DomainException("Pessoa já está inativa.");

        Situacao = false;
        UpdateTimestamp();
    }

    protected override string DescricaoFormatada => $"{Nome} ({CPF.NumeroFormatado})";

    private void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome é obrigatório.");

        if (nome.Length > 100)
            throw new DomainException("Nome deve ter no máximo 100 caracteres.");
    }

    private void ValidarTelefone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
            throw new DomainException("Telefone é obrigatório.");

        var telefoneNumerico = new string(telefone.Where(char.IsDigit).ToArray());

        if (telefoneNumerico.Length < 10 || telefoneNumerico.Length > 11)
            throw new DomainException("Telefone deve ter 10 ou 11 caracteres.");

        if (!telefoneNumerico.All(char.IsDigit))
            throw new DomainException("Telefone deve conter apenas números.");
    }

    private void ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("E-mail é obrigatório.");

        if (email.Length > 100)
            throw new DomainException("E-mail deve ter no máximo 100 caracteres.");

        if (!email.Contains("@") || !email.Contains("."))
            throw new DomainException("E-mail inválido.");
    }
}

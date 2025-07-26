using Dominio.Enums.Pessoas;
using Dominio.Exceptions;
using Dominio.Interfaces.Service.Pessoas;

namespace Dominio.Entidades.Pessoas;

public abstract class Pessoa : BaseEntity, IPessoa
{
    protected Pessoa() { } // Construtor protegido para EF Core

    protected Pessoa(
        string nome,
        string cpf,
        string telefone,
        string email,
        SexoEnum sexo,
        string observacao)
    {
        ValidarNome(nome);
        ValidarCPF(cpf);
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
    public string CPF { get; protected set; }
    public string Telefone { get; protected set; }
    public string Email { get; protected set; }
    public SexoEnum Sexo { get; protected set; }
    public long LocalidadeId { get; protected set; }
    public long LocalidadeEmbarqueId { get; protected set; }
    public long LocalidadeDesembarqueId { get; protected set; }
    public bool Situacao { get; protected set; }
    public string Observacao { get; protected set; }

    public virtual void Atualizar(
        string nome,
        string cpf,
        string telefone,
        string email,
        SexoEnum sexo,
        long localidadeId,
        long localidadeEmbarqueId,
        long localidadeDesembarqueId,
        string observacao)
    {
        ValidarNome(nome);
        ValidarCPF(cpf);
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
    }

    public void Ativar()
    {
        if (Situacao)
            throw new DomainException("Pessoa já está ativa.");

        Situacao = true;
    }

    public void Inativar()
    {
        if (!Situacao)
            throw new DomainException("Pessoa já está inativa.");

        Situacao = false;
    }

    public string CPF_Formatado => string.IsNullOrWhiteSpace(CPF) ? string.Empty : $"{CPF.Substring(0, 3)}.{CPF.Substring(3, 3)}.{CPF.Substring(6, 3)}-{CPF.Substring(9, 2)}";

    protected override string DescricaoFormatada => $"{Nome} ({CPF_Formatado})";

    private void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome é obrigatório.");

        if (nome.Length > 100)
            throw new DomainException("Nome deve ter no máximo 100 caracteres.");
    }

    private void ValidarCPF(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new DomainException("CPF é obrigatório.");

        if (cpf.Length != 11)
            throw new DomainException("CPF deve ter 11 caracteres.");

        if (!cpf.All(char.IsDigit))
            throw new DomainException("CPF deve conter apenas números.");

        if (!ValidarDigitosCPF(cpf))
            throw new DomainException("CPF inválido.");
    }

    private void ValidarTelefone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
            throw new DomainException("Telefone é obrigatório.");

        if (telefone.Length < 10 || telefone.Length > 11)
            throw new DomainException("Telefone deve ter 10 ou 11 caracteres.");

        if (!telefone.All(char.IsDigit))
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

    private bool ValidarDigitosCPF(string cpf)
    {
        if (cpf.Length != 11)
            return false;

        if (cpf.All(c => c == cpf[0]))
            return false;

        var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCpf = cpf.Substring(0, 9);
        var soma = 0;

        for (var i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        var resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        var digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;

        for (var i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = digito + resto.ToString();

        return cpf.EndsWith(digito);
    }
}
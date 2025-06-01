using Dominio.Enums.Pessoas;
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
        string observacao,
        bool situacao)
    {
        Nome = nome;
        CPF = cpf;
        Telefone = telefone;
        Email = email;
        Sexo = sexo;
        Observacao = observacao;
        Situacao = situacao;
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
        string observacao,
        bool situacao)
    {
        Nome = nome;
        CPF = cpf;
        Telefone = telefone;
        Email = email;
        Sexo = sexo;
        LocalidadeId = localidadeId;
        LocalidadeEmbarqueId = localidadeEmbarqueId;
        LocalidadeDesembarqueId = localidadeDesembarqueId;
        Observacao = observacao;
        Situacao = situacao;
    }

    protected override string DescricaoFormatada => $"{Nome} ({CPF})";
}
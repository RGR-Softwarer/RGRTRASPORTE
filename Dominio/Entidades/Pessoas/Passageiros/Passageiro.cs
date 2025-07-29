using Dominio.Entidades.Localidades;
using Dominio.Enums.Pessoas;
using Dominio.Events.Base;
using Dominio.ValueObjects;

namespace Dominio.Entidades.Pessoas.Passageiros;

public class Passageiro : Pessoa
{
    protected Passageiro() { } // Construtor protegido para EF Core

    public Passageiro(
        string nome,
        CPF cpf,
        string telefone,
        string email,
        SexoEnum sexo,
        long localidadeId,
        long localidadeEmbarqueId,
        long localidadeDesembarqueId,
        string observacao,
        bool situacao) : base(nome, cpf, telefone, email, sexo, observacao)
    {
        LocalidadeId = localidadeId;
        LocalidadeEmbarqueId = localidadeEmbarqueId;
        LocalidadeDesembarqueId = localidadeDesembarqueId;
        Situacao = situacao;

        AddDomainEvent(new PassageiroCriadoEvent(Id, nome, cpf.Numero));
    }

    public Localidade Localidade { get; private set; }
    public new long LocalidadeId { get; private set; }
    public Localidade LocalidadeEmbarque { get; private set; }
    public new long? LocalidadeEmbarqueId { get; private set; }
    public Localidade LocalidadeDesembarque { get; private set; }
    public new long? LocalidadeDesembarqueId { get; private set; }

    public void Atualizar(
        string nome,
        CPF cpf,
        string telefone,
        string email,
        SexoEnum sexo,
        long localidadeId,
        long localidadeEmbarqueId,
        long localidadeDesembarqueId,
        string observacao,
        bool situacao)
    {
        base.Atualizar(nome, cpf, telefone, email, sexo, localidadeId, localidadeEmbarqueId, localidadeDesembarqueId, observacao);
        Situacao = situacao;
        UpdateTimestamp();

        AddDomainEvent(new PassageiroAtualizadoEvent(Id, nome, cpf.Numero));
    }

    public void AtualizarLocalidades(
        long localidadeId,
        long localidadeEmbarqueId,
        long localidadeDesembarqueId)
    {
        LocalidadeId = localidadeId;
        LocalidadeEmbarqueId = localidadeEmbarqueId;
        LocalidadeDesembarqueId = localidadeDesembarqueId;
        UpdateTimestamp();

        AddDomainEvent(new PassageiroLocalidadesAtualizadasEvent(Id, Nome, CPF.Numero));
    }
}

// Eventos de domínio para Passageiro
public class PassageiroCriadoEvent : DomainEvent
{
    public long PassageiroId { get; }
    public string Nome { get; }
    public string CPF { get; }

    public PassageiroCriadoEvent(long passageiroId, string nome, string cpf)
    {
        PassageiroId = passageiroId;
        Nome = nome;
        CPF = cpf;
    }
}

public class PassageiroAtualizadoEvent : DomainEvent
{
    public long PassageiroId { get; }
    public string Nome { get; }
    public string CPF { get; }

    public PassageiroAtualizadoEvent(long passageiroId, string nome, string cpf)
    {
        PassageiroId = passageiroId;
        Nome = nome;
        CPF = cpf;
    }
}

public class PassageiroLocalidadesAtualizadasEvent : DomainEvent
{
    public long PassageiroId { get; }
    public string Nome { get; }
    public string CPF { get; }

    public PassageiroLocalidadesAtualizadasEvent(long passageiroId, string nome, string cpf)
    {
        PassageiroId = passageiroId;
        Nome = nome;
        CPF = cpf;
    }
}

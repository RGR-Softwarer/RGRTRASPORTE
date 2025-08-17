using Dominio.Entidades.Localidades;
using Dominio.Enums.Pessoas;
using Dominio.Events.Base;
using Dominio.ValueObjects;
using Dominio.Interfaces;
using Dominio.Services;
using Dominio.Exceptions;

namespace Dominio.Entidades.Pessoas.Passageiros;

public class Passageiro : Pessoa
{
    private Passageiro() { } // Para EF Core

    // Construtor privado - usar Factory Methods
    private Passageiro(
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

    // Factory Methods
    public static Passageiro CriarPassageiro(
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
        return new Passageiro(nome, cpf, telefone, email, sexo, localidadeId, 
            localidadeEmbarqueId, localidadeDesembarqueId, observacao, true);
    }

    // Factory Method com validação por NotificationContext
    public static (Passageiro? passageiro, bool sucesso) CriarPassageiroComValidacao(
        string nome,
        CPF cpf,
        string telefone,
        string email,
        SexoEnum sexo,
        long localidadeId,
        long localidadeEmbarqueId,
        long localidadeDesembarqueId,
        string observacao,
        IDomainNotificationContext notificationContext)
    {
        var validationService = new PassageiroValidationService();
        var valido = validationService.ValidarCriacao(nome, cpf, telefone, email, sexo,
            localidadeId, localidadeEmbarqueId, localidadeDesembarqueId, observacao, notificationContext);

        if (!valido)
            return (null, false);

        try
        {
            var passageiro = CriarPassageiro(nome, cpf, telefone, email, sexo, localidadeId, 
                localidadeEmbarqueId, localidadeDesembarqueId, observacao);
            return (passageiro, true);
        }
        catch (DomainException ex)
        {
            notificationContext.AddNotification(ex.Message);
            return (null, false);
        }
    }

    public Localidade Localidade { get; private set; } = null!;
    public new long LocalidadeId { get; private set; }
    public Localidade LocalidadeEmbarque { get; private set; } = null!;
    public new long? LocalidadeEmbarqueId { get; private set; }
    public Localidade LocalidadeDesembarque { get; private set; } = null!;
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

// Eventos de dom�nio para Passageiro
public class PassageiroCriadoEvent : DomainEvent
{
    public long PassageiroId { get; }
    public string Nome { get; } = null!;
    public string CPF { get; } = null!;

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
    public string Nome { get; } = null!;
    public string CPF { get; } = null!;

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
    public string Nome { get; } = null!;
    public string CPF { get; } = null!;

    public PassageiroLocalidadesAtualizadasEvent(long passageiroId, string nome, string cpf)
    {
        PassageiroId = passageiroId;
        Nome = nome;
        CPF = cpf;
    }
}

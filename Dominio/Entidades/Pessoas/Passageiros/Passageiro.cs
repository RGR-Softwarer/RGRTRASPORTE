using Dominio.Entidades.Localidades;
using Dominio.Enums.Pessoas;

namespace Dominio.Entidades.Pessoas.Passageiros;

public class Passageiro : Pessoa
{
    protected Passageiro() { } // Construtor protegido para EF Core

    public Passageiro(
        string nome,
        string cpf,
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
    }

    public Localidade Localidade { get; private set; }
    public new long LocalidadeId { get; private set; }
    public Localidade LocalidadeEmbarque { get; private set; }
    public new long? LocalidadeEmbarqueId { get; private set; }
    public Localidade LocalidadeDesembarque { get; private set; }
    public new long? LocalidadeDesembarqueId { get; private set; }

    public void Atualizar(
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
        base.Atualizar(nome, cpf, telefone, email, sexo, localidadeId, localidadeEmbarqueId, localidadeDesembarqueId, observacao);
        Situacao = situacao;
    }

    public void AtualizarLocalidades(
        long localidadeId,
        long localidadeEmbarqueId,
        long localidadeDesembarqueId)
    {
        LocalidadeId = localidadeId;
        LocalidadeEmbarqueId = localidadeEmbarqueId;
        LocalidadeDesembarqueId = localidadeDesembarqueId;
    }
}

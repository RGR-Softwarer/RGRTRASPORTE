using Application.Commands.Base;
using Application.Common;
using Dominio.Enums.Pessoas;

namespace Application.Commands.Passageiro;

public class EditarPassageiroCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }
    public string Nome { get; private set; }
    public string CPF { get; private set; }
    public string Telefone { get; private set; }
    public string Email { get; private set; }
    public SexoEnum Sexo { get; private set; }
    public long LocalidadeId { get; private set; }
    public long LocalidadeEmbarqueId { get; private set; }
    public long LocalidadeDesembarqueId { get; private set; }
    public string Observacao { get; private set; }
    public bool Situacao { get; private set; }

    public EditarPassageiroCommand(
        long id,
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
        Id = id;
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
} 
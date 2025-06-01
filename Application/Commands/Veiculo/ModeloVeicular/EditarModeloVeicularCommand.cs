using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Veiculo.ModeloVeicular;

public class EditarModeloVeicularCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }
    public string Descricao { get; private set; }
    public int QuantidadePassageiros { get; private set; }
    public bool Ativo { get; private set; }

    public EditarModeloVeicularCommand(
        long id,
        string nome,
        string descricao,
        int quantidadePassageiros,
        bool ativo,
        string usuarioId,
        string usuarioCriacao) : base(usuarioId, usuarioCriacao)
    {
        Id = id;
        Descricao = descricao;
        QuantidadePassageiros = quantidadePassageiros;
        Ativo = ativo;
    }
} 
using Application.Commands.Base;
using Application.Common;
using Dominio.Enums.Veiculo;

namespace Application.Commands.Veiculo.ModeloVeicular;

public class EditarModeloVeicularCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }
    public string Descricao { get; private set; }
    public TipoModeloVeiculoEnum Tipo { get; private set; }
    public int QuantidadeAssento { get; private set; }
    public int QuantidadeEixo { get; private set; }
    public int CapacidadeMaxima { get; private set; }
    public int PassageirosEmPe { get; private set; }
    public bool PossuiBanheiro { get; private set; }
    public bool PossuiClimatizador { get; private set; }
    public bool Situacao { get; private set; }

    public EditarModeloVeicularCommand(
        long id,
        string descricao,
        TipoModeloVeiculoEnum tipo,
        int quantidadeAssento,
        int quantidadeEixo,
        int capacidadeMaxima,
        int passageirosEmPe,
        bool possuiBanheiro,
        bool possuiClimatizador,
        bool situacao,
        bool ativo)
    {
        Id = id;
        Descricao = descricao;
        Tipo = tipo;
        QuantidadeAssento = quantidadeAssento;
        QuantidadeEixo = quantidadeEixo;
        CapacidadeMaxima = capacidadeMaxima;
        PassageirosEmPe = passageirosEmPe;
        PossuiBanheiro = possuiBanheiro;
        PossuiClimatizador = possuiClimatizador;
        Situacao = situacao;
    }
} 

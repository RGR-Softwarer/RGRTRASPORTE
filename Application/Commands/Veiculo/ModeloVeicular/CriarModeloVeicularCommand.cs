using Application.Commands.Base;
using Application.Common;
using Dominio.Enums.Veiculo;

namespace Application.Commands.Veiculo.ModeloVeicular;

public class CriarModeloVeicularCommand : BaseCommand<BaseResponse<long>>
{
    public string Descricao { get; set; }
    public bool Situacao { get; private set; }
    public TipoModeloVeiculoEnum Tipo { get; private set; }
    public int QuantidadeAssento { get; private set; }
    public int QuantidadeEixo { get; private set; }
    public int CapacidadeMaxima { get; private set; }
    public int PassageirosEmPe { get; private set; }
    public bool PossuiBanheiro { get; private set; }
    public bool PossuiClimatizador { get; private set; }

    public CriarModeloVeicularCommand(string descricao, bool situacao, TipoModeloVeiculoEnum tipo, int quantidadeAssento, int quantidadeEixo, int capacidadeMaxima, int passageirosEmPe, bool possuiBanheiro, bool possuiClimatizador)
    {
        Descricao = descricao;
        Situacao = situacao;
        Tipo = tipo;
        QuantidadeAssento = quantidadeAssento;
        QuantidadeEixo = quantidadeEixo;
        CapacidadeMaxima = capacidadeMaxima;
        PassageirosEmPe = passageirosEmPe;
        PossuiBanheiro = possuiBanheiro;
        PossuiClimatizador = possuiClimatizador;
    }
} 
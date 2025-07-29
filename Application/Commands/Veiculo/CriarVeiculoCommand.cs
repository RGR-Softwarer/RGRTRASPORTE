using Application.Commands.Base;
using Application.Common;
using Dominio.Enums.Veiculo;

namespace Application.Commands.Veiculo;

public class CriarVeiculoCommand : BaseCommand<BaseResponse<long>>
{
    public string Placa { get; private set; }
    public string Modelo { get; private set; }
    public string Marca { get; private set; }
    public string NumeroChassi { get; private set; }
    public int AnoModelo { get; private set; }
    public int AnoFabricacao { get; private set; }
    public string Cor { get; private set; }
    public string Renavam { get; private set; }
    public DateTime? VencimentoLicenciamento { get; private set; }
    public TipoCombustivelEnum TipoCombustivel { get; private set; }
    public StatusVeiculoEnum Status { get; private set; }
    public string Observacao { get; private set; }
    public long? ModeloVeiculoId { get; private set; }

    public CriarVeiculoCommand(
        string placa,
        string modelo,
        string marca,
        string numeroChassi,
        int anoModelo,
        int anoFabricacao,
        string cor,
        string renavam,
        DateTime? vencimentoLicenciamento,
        TipoCombustivelEnum tipoCombustivel,
        StatusVeiculoEnum status,
        string observacao,
        long? modeloVeiculoId)
    {
        Placa = placa;
        Modelo = modelo;
        Marca = marca;
        NumeroChassi = numeroChassi;
        AnoModelo = anoModelo;
        AnoFabricacao = anoFabricacao;
        Cor = cor;
        Renavam = renavam;
        VencimentoLicenciamento = vencimentoLicenciamento;
        TipoCombustivel = tipoCombustivel;
        Status = status;
        Observacao = observacao;
        ModeloVeiculoId = modeloVeiculoId;
    }
} 
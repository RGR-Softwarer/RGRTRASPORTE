using Application.Commands.Base;
using Application.Common;
using Dominio.Enums.Veiculo;

namespace Application.Commands.Veiculo;

public class VeiculoLoteDto
{
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public string Marca { get; set; }
    public string NumeroChassi { get; set; }
    public int AnoModelo { get; set; }
    public int AnoFabricacao { get; set; }
    public string Cor { get; set; }
    public string Renavam { get; set; }
    public DateTime? VencimentoLicenciamento { get; set; }
    public TipoCombustivelEnum TipoCombustivel { get; set; }
    public StatusVeiculoEnum Status { get; set; }
    public string Observacao { get; set; }
    public long? ModeloVeiculoId { get; set; }
}

public class AdicionarVeiculosEmLoteCommand : BaseCommand<BaseResponse<IEnumerable<long>>>
{
    public IEnumerable<VeiculoLoteDto> Veiculos { get; private set; }

    public AdicionarVeiculosEmLoteCommand(
        IEnumerable<VeiculoLoteDto> veiculos,
        string usuarioId,
        string usuarioCriacao) : base(usuarioId, usuarioCriacao)
    {
        Veiculos = veiculos;
    }
} 
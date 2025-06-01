using Application.Commands.Base;
using Application.Common;
using Dominio.Enums.Veiculo;

namespace Application.Commands.Veiculo;

public class VeiculoLoteEdicaoDto : VeiculoLoteDto
{
    public long Id { get; set; }
}

public class EditarVeiculosEmLoteCommand : BaseCommand<BaseResponse<bool>>
{
    public IEnumerable<VeiculoLoteEdicaoDto> Veiculos { get; private set; }

    public EditarVeiculosEmLoteCommand(
        IEnumerable<VeiculoLoteEdicaoDto> veiculos,
        string usuarioId,
        string usuarioCriacao) : base(usuarioId, usuarioCriacao)
    {
        Veiculos = veiculos;
    }
} 
using Dominio.Dtos;
using Dominio.Dtos.Veiculo;
using MediatR;

namespace Application.Commands.Veiculo
{
    public class ObterTodosVeiculosQuery : IRequest<IEnumerable<VeiculoDto>> { }

    public class ObterVeiculoPorIdQuery : IRequest<VeiculoDto>
    {
        public long Id { get; set; }
        public ObterVeiculoPorIdQuery(long id) => Id = id;
    }

    public class ObterVeiculosPaginadoQuery : IRequest<ResponseGridDto<VeiculoDto>>
    {
        public ParametrosBuscaDto Parametros { get; set; }

        public ObterVeiculosPaginadoQuery(ParametrosBuscaDto parametros)
        {
            Parametros = parametros;
        }
    }

    public class AdicionarVeiculoCommand : IRequest<bool>
    {
        public VeiculoDto VeiculoDto { get; set; }
    }

    public class AdicionarVeiculosEmLoteCommand : IRequest<bool>
    {
        public List<VeiculoDto> VeiculosDto { get; set; }
    }

    public class EditarVeiculoCommand : IRequest<bool>
    {
        public VeiculoDto VeiculoDto { get; set; }
    }

    public class EditarVeiculosEmLoteCommand : IRequest<bool>
    {
        public List<VeiculoDto> VeiculosDto { get; set; }
    }

    public class RemoverVeiculoCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public RemoverVeiculoCommand(long id) => Id = id;
    }

    // Modelo Veicular
    public class ObterTodosModelosVeicularesQuery : IRequest<IEnumerable<ModeloVeicularDto>> { }

    public class ObterModeloVeicularPorIdQuery : IRequest<ModeloVeicularDto>
    {
        public long Id { get; set; }
        public ObterModeloVeicularPorIdQuery(long id) => Id = id;
    }

    public class AdicionarModeloVeicularCommand : IRequest<bool>
    {
        public ModeloVeicularDto ModeloVeicularDto { get; set; }
    }

    public class EditarModeloVeicularCommand : IRequest<bool>
    {
        public ModeloVeicularDto ModeloVeicularDto { get; set; }
    }

    public class RemoverModeloVeicularCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public RemoverModeloVeicularCommand(long id) => Id = id;
    }
} 
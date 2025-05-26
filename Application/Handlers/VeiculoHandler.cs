using Application.Commands.Veiculo;
using Dominio.Dtos.Veiculo;
using Dominio.Interfaces.Service;
using MediatR;

namespace Application.Handlers
{
    public class VeiculoHandler :
        IRequestHandler<ObterTodosVeiculosQuery, IEnumerable<VeiculoDto>>,
        IRequestHandler<ObterVeiculoPorIdQuery, VeiculoDto>,
        IRequestHandler<AdicionarVeiculoCommand, bool>,
        IRequestHandler<AdicionarVeiculosEmLoteCommand, bool>,
        IRequestHandler<EditarVeiculoCommand, bool>,
        IRequestHandler<EditarVeiculosEmLoteCommand, bool>,
        IRequestHandler<RemoverVeiculoCommand, bool>,
        IRequestHandler<ObterTodosModelosVeicularesQuery, IEnumerable<ModeloVeicularDto>>,
        IRequestHandler<ObterModeloVeicularPorIdQuery, ModeloVeicularDto>,
        IRequestHandler<AdicionarModeloVeicularCommand, bool>,
        IRequestHandler<EditarModeloVeicularCommand, bool>,
        IRequestHandler<RemoverModeloVeicularCommand, bool>
    {
        private readonly IVeiculoService _veiculoService;
        private readonly IModeloVeicularService _modeloVeicularService;

        public VeiculoHandler(IVeiculoService veiculoService, IModeloVeicularService modeloVeicularService)
        {
            _veiculoService = veiculoService;
            _modeloVeicularService = modeloVeicularService;
        }

        public async Task<IEnumerable<VeiculoDto>> Handle(ObterTodosVeiculosQuery request, CancellationToken cancellationToken)
        {
            return await _veiculoService.ObterTodosAsync();
        }

        public async Task<VeiculoDto> Handle(ObterVeiculoPorIdQuery request, CancellationToken cancellationToken)
        {
            return await _veiculoService.ObterPorIdAsync(request.Id);
        }

        public async Task<bool> Handle(AdicionarVeiculoCommand request, CancellationToken cancellationToken)
        {
            await _veiculoService.AdicionarAsync(request.VeiculoDto);
            return true;
        }

        public async Task<bool> Handle(AdicionarVeiculosEmLoteCommand request, CancellationToken cancellationToken)
        {
            await _veiculoService.AdicionarEmLoteAsync(request.VeiculosDto);
            return true;
        }

        public async Task<bool> Handle(EditarVeiculoCommand request, CancellationToken cancellationToken)
        {
            await _veiculoService.EditarAsync(request.VeiculoDto);
            return true;
        }

        public async Task<bool> Handle(EditarVeiculosEmLoteCommand request, CancellationToken cancellationToken)
        {
            await _veiculoService.EditarEmLoteAsync(request.VeiculosDto);
            return true;
        }

        public async Task<bool> Handle(RemoverVeiculoCommand request, CancellationToken cancellationToken)
        {
            await _veiculoService.RemoverAsync(request.Id);
            return true;
        }

        public async Task<IEnumerable<ModeloVeicularDto>> Handle(ObterTodosModelosVeicularesQuery request, CancellationToken cancellationToken)
        {
            return await _modeloVeicularService.ObterTodosAsync();
        }

        public async Task<ModeloVeicularDto> Handle(ObterModeloVeicularPorIdQuery request, CancellationToken cancellationToken)
        {
            return await _modeloVeicularService.ObterPorIdAsync(request.Id);
        }

        public async Task<bool> Handle(AdicionarModeloVeicularCommand request, CancellationToken cancellationToken)
        {
            await _modeloVeicularService.AdicionarAsync(request.ModeloVeicularDto);
            return true;
        }

        public async Task<bool> Handle(EditarModeloVeicularCommand request, CancellationToken cancellationToken)
        {
            await _modeloVeicularService.EditarAsync(request.ModeloVeicularDto);
            return true;
        }

        public async Task<bool> Handle(RemoverModeloVeicularCommand request, CancellationToken cancellationToken)
        {
            await _modeloVeicularService.RemoverAsync(request.Id);
            return true;
        }
    }
} 
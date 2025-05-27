using Application.Commands.Viagem;
using Dominio.Dtos.Viagens;
using Dominio.Interfaces.Service.Viagens;
using MediatR;

namespace Application.Handlers
{
    public class ViagemHandler : 
        IRequestHandler<ObterTodasViagensQuery, IEnumerable<ViagemDto>>,
        IRequestHandler<ObterViagemPorIdQuery, ViagemDto>,
        IRequestHandler<AdicionarViagemCommand, bool>,
        IRequestHandler<EditarViagemCommand, bool>,
        IRequestHandler<RemoverViagemCommand, bool>
    {
        private readonly IViagemService _viagemService;

        public ViagemHandler(IViagemService viagemService)
        {
            _viagemService = viagemService;
        }

        public async Task<IEnumerable<ViagemDto>> Handle(ObterTodasViagensQuery request, CancellationToken cancellationToken)
        {
            return await _viagemService.ObterTodosAsync();
        }

        public async Task<ViagemDto> Handle(ObterViagemPorIdQuery request, CancellationToken cancellationToken)
        {
            return await _viagemService.ObterPorIdAsync(request.Id);
        }

        public async Task<bool> Handle(AdicionarViagemCommand request, CancellationToken cancellationToken)
        {
            await _viagemService.AdicionarAsync(new ViagemDto() { });//request.ViagemDto);
            return true;
        }

        public async Task<bool> Handle(EditarViagemCommand request, CancellationToken cancellationToken)
        {
            await _viagemService.EditarAsync(request.ViagemDto);
            return true;
        }

        public async Task<bool> Handle(RemoverViagemCommand request, CancellationToken cancellationToken)
        {
            await _viagemService.RemoverAsync(request.Id);
            return true;
        }
    }
} 
using Application.Commands.Passageiro;
using Dominio.Dtos.Pessoas.Passageiros;
using Dominio.Interfaces.Service.Passageiros;
using MediatR;

namespace Application.Handlers
{
    public class PassageiroHandler :
        IRequestHandler<ObterTodosPassageirosQuery, IEnumerable<PassageiroDto>>,
        IRequestHandler<ObterPassageiroPorIdQuery, PassageiroDto>,
        IRequestHandler<AdicionarPassageiroCommand, bool>,
        IRequestHandler<EditarPassageiroCommand, bool>,
        IRequestHandler<RemoverPassageiroCommand, bool>
    {
        private readonly IPassageiroService _passageiroService;

        public PassageiroHandler(IPassageiroService passageiroService)
        {
            _passageiroService = passageiroService;
        }

        public async Task<IEnumerable<PassageiroDto>> Handle(ObterTodosPassageirosQuery request, CancellationToken cancellationToken)
        {
            return await _passageiroService.ObterTodosAsync();
        }

        public async Task<PassageiroDto> Handle(ObterPassageiroPorIdQuery request, CancellationToken cancellationToken)
        {
            return await _passageiroService.ObterPorIdAsync(request.Id);
        }

        public async Task<bool> Handle(AdicionarPassageiroCommand request, CancellationToken cancellationToken)
        {
            await _passageiroService.AdicionarAsync(request.PassageiroDto);
            return true;
        }

        public async Task<bool> Handle(EditarPassageiroCommand request, CancellationToken cancellationToken)
        {
            await _passageiroService.EditarAsync(request.PassageiroDto);
            return true;
        }

        public async Task<bool> Handle(RemoverPassageiroCommand request, CancellationToken cancellationToken)
        {
            await _passageiroService.RemoverAsync(request.Id);
            return true;
        }
    }
} 
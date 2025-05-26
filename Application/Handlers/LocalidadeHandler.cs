using Application.Commands.Localidade;
using Dominio.Dtos.Localidades;
using Dominio.Interfaces.Service.Localidades;
using MediatR;

namespace Application.Handlers
{
    public class LocalidadeHandler :
        IRequestHandler<ObterTodasLocalidadesQuery, IEnumerable<LocalidadeDto>>,
        IRequestHandler<ObterLocalidadePorIdQuery, LocalidadeDto>,
        IRequestHandler<AdicionarLocalidadeCommand, bool>,
        IRequestHandler<EditarLocalidadeCommand, bool>,
        IRequestHandler<RemoverLocalidadeCommand, bool>
    {
        private readonly ILocalidadeService _localidadeService;

        public LocalidadeHandler(ILocalidadeService localidadeService)
        {
            _localidadeService = localidadeService;
        }

        public async Task<IEnumerable<LocalidadeDto>> Handle(ObterTodasLocalidadesQuery request, CancellationToken cancellationToken)
        {
            return await _localidadeService.ObterTodosAsync();
        }

        public async Task<LocalidadeDto> Handle(ObterLocalidadePorIdQuery request, CancellationToken cancellationToken)
        {
            return await _localidadeService.ObterPorIdAsync(request.Id);
        }

        public async Task<bool> Handle(AdicionarLocalidadeCommand request, CancellationToken cancellationToken)
        {
            await _localidadeService.AdicionarAsync(request.LocalidadeDto);
            return true;
        }

        public async Task<bool> Handle(EditarLocalidadeCommand request, CancellationToken cancellationToken)
        {
            await _localidadeService.EditarAsync(request.LocalidadeDto);
            return true;
        }

        public async Task<bool> Handle(RemoverLocalidadeCommand request, CancellationToken cancellationToken)
        {
            await _localidadeService.RemoverAsync(request.Id);
            return true;
        }
    }
} 
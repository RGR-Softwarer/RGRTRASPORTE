using Application.Events.Viagem;
using AutoMapper;
using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;

namespace Application.Commands.ViagemPassageiro
{
    public class AdicionarViagemHandler : IRequestHandler<AdicionarViagemCommand, Unit>
    {

        private readonly IViagemRepository _viagemRepository;
        private readonly IMapper _mapper;
        private readonly INotificationHandler _notificationHandler;
        private readonly IMediator _mediator;

        public AdicionarViagemHandler(IMediator mediator, IViagemRepository viagemRepository, IMapper mapper, INotificationHandler notificationHandler)
        {
            _viagemRepository = viagemRepository;
            _mapper = mapper;
            _notificationHandler = notificationHandler;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AdicionarViagemCommand request, CancellationToken cancellationToken)
        {
            var viagem = _mapper.Map<Viagem>(request);
            await _viagemRepository.AdicionarAsync(viagem);

            await _mediator.Publish(new ViagemCriadaEvent(viagem.Id), cancellationToken);

            return Unit.Value;
        }
    }
}
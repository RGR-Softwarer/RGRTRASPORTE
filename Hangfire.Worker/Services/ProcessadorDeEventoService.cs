using Dominio.Interfaces.Hangfire;
using MediatR;

namespace Hangfire.Worker.Services
{
    public class ProcessadorDeEventoService : IProcessadorDeEventoService
    {
        private readonly IMediator _mediator;

        public ProcessadorDeEventoService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task ProcessarViagemCriada(long viagemId)
        {
            // Reenvia para um CommandHandler do Application
            //await _mediator.Send(new ProcessarViagemCriadaCommand { ViagemId = viagemId });
        }
    }
}
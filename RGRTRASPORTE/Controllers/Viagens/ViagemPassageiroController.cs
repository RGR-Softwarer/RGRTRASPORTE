using Application.Commands.Viagem;
using Application.Queries.Viagem.ViagemPassageiro;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Viagens
{
    [Route("api/Viagem/{viagemId}/passageiros")]
    [ApiController]
    public class ViagemPassageiroController : AbstractControllerBase
    {
        public ViagemPassageiroController(IMediator mediator, INotificationContext notificationHandler) : base(notificationHandler, mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos(long viagemId, [FromQuery] ObterViagemPassageirosQuery query, CancellationToken cancellationToken)
        {
            query.ViagemId = viagemId; // Garante que o ID da rota seja usado
            var viagemPassageiros = await _mediator.Send(query, cancellationToken);
            return await RGRResult(System.Net.HttpStatusCode.OK, viagemPassageiros);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar(long viagemId, [FromBody] AdicionarPassageiroViagemApiCommand command, CancellationToken cancellationToken)
        {
            command.ViagemId = viagemId; // Garante que o ID da rota seja usado
            var result = await _mediator.Send(command, cancellationToken);
            return await RGRResult(System.Net.HttpStatusCode.Created, result);
        }

        [HttpDelete("{passageiroId}")]
        public async Task<IActionResult> Remover(long viagemId, long passageiroId, CancellationToken cancellationToken)
        {
            var command = new RemoverPassageiroViagemCommand(viagemId, passageiroId);
            var result = await _mediator.Send(command, cancellationToken);
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }
    }
}

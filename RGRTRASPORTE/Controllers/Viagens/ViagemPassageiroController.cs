using Application.Commands.Viagem;
using Application.Queries.Viagem.ViagemPassageiro;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Viagens
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViagemPassageiroController : AbstractControllerBase
    {
        private readonly IMediator _mediator;

        public ViagemPassageiroController(
            IMediator mediator,
            INotificationContext notificationHandler)
            : base(notificationHandler)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos([FromQuery] ObterViagemPassageirosQuery query)
        {
            var viagemPassageiros = await _mediator.Send(query);
            return await RGRResult(System.Net.HttpStatusCode.OK, viagemPassageiros);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] AdicionarPassageiroViagemCommand command)
        {
            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.Created, result);
        }

        [HttpDelete("{viagemId}/{passageiroId}")]
        public async Task<IActionResult> Remover(long viagemId, long passageiroId)
        {
            var command = new RemoverPassageiroViagemCommand(viagemId, passageiroId);
            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }
    }
} 
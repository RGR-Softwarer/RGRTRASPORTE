using Application.Commands.Passageiro;
using Application.Queries.Passageiro;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Pessoas
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassageiroController : AbstractControllerBase
    {
        private readonly IMediator _mediator;

        public PassageiroController(
            IMediator mediator,
            INotificationContext notificationHandler)
            : base(notificationHandler)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos([FromQuery] ObterPassageirosQuery query)
        {
            var passageiros = await _mediator.Send(query);
            return await RGRResult(System.Net.HttpStatusCode.OK, passageiros);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id, [FromQuery] bool auditado = false)
        {
            var passageiro = await _mediator.Send(new ObterPassageiroPorIdQuery(id, auditado));
            if (passageiro == null)
                return await RGRResult(System.Net.HttpStatusCode.NotFound, $"Passageiro com ID {id} não encontrado");

            return await RGRResult(System.Net.HttpStatusCode.OK, passageiro);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] CriarPassageiroCommand command)
        {
            var id = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.Created, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(long id, [FromBody] EditarPassageiroCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id da rota diferente do Id do comando");

            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(long id)
        {
            var command = new RemoverPassageiroCommand(id);
            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }
    }
}

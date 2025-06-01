using Application.Commands.Viagem.ViagemPosicao;
using Application.Queries.Viagem.ViagemPosicao;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Viagens
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViagemPosicaoController : AbstractControllerBase
    {
        private readonly IMediator _mediator;

        public ViagemPosicaoController(
            IMediator mediator,
            INotificationContext notificationHandler)
            : base(notificationHandler)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos([FromQuery] ObterViagemPosicoesQuery query)
        {
            var viagemPosicoes = await _mediator.Send(query);
            return await RGRResult(System.Net.HttpStatusCode.OK, viagemPosicoes);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] AdicionarViagemPosicaoCommand command)
        {
            var id = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.Created, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(long id, [FromBody] EditarViagemPosicaoCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id da rota diferente do Id do comando");

            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(long id)
        {
            var result = await _mediator.Send(new RemoverViagemPosicaoCommand(id, string.Empty, string.Empty));
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }
    }
}
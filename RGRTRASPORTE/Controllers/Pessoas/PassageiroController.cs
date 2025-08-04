using Application.Commands.Passageiro;
using Application.Queries.Passageiros;
using Application.Queries.Passageiros.Models;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Pessoas
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassageiroController : AbstractControllerBase
    {
        public PassageiroController(IMediator mediator, INotificationContext notificationHandler) : base(notificationHandler, mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos([FromBody] ObterPassageirosQuery query)
        {
            var resultado = await _mediator.Send(query);

            if (!resultado.Sucesso)
                return BadRequest(resultado);

            return await RGRResult(System.Net.HttpStatusCode.OK, resultado);
        }

        [HttpPost("filtrar")]
        public async Task<IActionResult> ObterPassageirosPaginados([FromBody] ObterPassageirosPaginadosQuery query)
        {
            return await ObterPaginado<ObterPassageirosPaginadosQuery, PassageiroDto>(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var passageiro = await _mediator.Send(new ObterPassageiroPorIdQuery(id));
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

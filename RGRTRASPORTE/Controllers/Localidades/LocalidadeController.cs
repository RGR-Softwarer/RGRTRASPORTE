using Application.Commands.Localidade;
using Application.Queries.Localidade;
using Application.Queries.Localidade.Models;
using Dominio.Dtos;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Localidades
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalidadeController : AbstractControllerBase
    {
        private readonly IMediator _mediator;

        public LocalidadeController(
            IMediator mediator,
            INotificationContext notificationHandler)
            : base(notificationHandler)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos([FromQuery] string nome = null, [FromQuery] string estado = null, [FromQuery] bool? ativo = null)
        {
            var query = new ObterLocalidadesQuery
            {
                Nome = nome,
                Estado = estado,
                Ativo = ativo
            };

            var response = await _mediator.Send(query);
            return await RGRResult(System.Net.HttpStatusCode.OK, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var query = new ObterLocalidadePorIdQuery(id);
            var response = await _mediator.Send(query);
            
            if (response == null || !response.Sucesso)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, response);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] CriarLocalidadeCommand command)
        {
            var response = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.Created, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(long id, [FromBody] EditarLocalidadeCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id da rota diferente do Id do comando");

            var response = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(long id)
        {
            var command = new RemoverLocalidadeCommand(id, User.Identity.Name, User.Identity.Name);
            var response = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, response);
        }
    }
}
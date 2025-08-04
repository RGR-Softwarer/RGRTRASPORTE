using Application.Commands.Veiculo.ModeloVeicular;
using Application.Queries.Veiculo.ModeloVeicular;
using Application.Queries.Veiculo.ModeloVeicular.Models;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Veiculos
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModeloVeicularController : AbstractControllerBase
    {
        public ModeloVeicularController(IMediator mediator, INotificationContext notificationHandler) : base(notificationHandler, mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos([FromQuery] ObterModelosVeicularesQuery query)
        {                
            var resultado = await _mediator.Send(query);
            
            if (!resultado.Sucesso)
                return BadRequest(resultado);
                
            return await RGRResult(System.Net.HttpStatusCode.OK, resultado);
        }

        [HttpPost("filtrar")]
        public async Task<IActionResult> ObterModelosVeicularesPaginados([FromBody] ObterModelosVeicularesPaginadosQuery query)
        {
            return await ObterPaginado<ObterModelosVeicularesPaginadosQuery, ModeloVeicularDto>(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var modelo = await _mediator.Send(new ObterModeloVeicularPorIdQuery(id));
            if (modelo == null)
                return await RGRResult(System.Net.HttpStatusCode.NotFound, $"Modelo veicular com ID {id} não encontrado");

            return await RGRResult(System.Net.HttpStatusCode.OK, modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] CriarModeloVeicularCommand command)
        {
            var id = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.Created, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(long id, [FromBody] EditarModeloVeicularCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id da rota diferente do Id do comando");

            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(long id)
        {
            var command = new RemoverModeloVeicularCommand(id);
            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }

        [HttpPost("seed")]
        public async Task<IActionResult> PopularDadosTeste([FromBody] SeedModeloVeicularCommand command)
        {
            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }
    }
}

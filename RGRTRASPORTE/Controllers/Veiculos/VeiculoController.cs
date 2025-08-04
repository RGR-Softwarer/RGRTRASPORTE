using Application.Commands.Veiculo;
using Application.Queries.Veiculo;
using Application.Queries.Veiculo.Models;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Veiculos
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculoController : AbstractControllerBase
    {
        public VeiculoController(IMediator mediator, INotificationContext notificationHandler) : base(notificationHandler, mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos([FromQuery] ObterVeiculosQuery query)
        {                  
            var resultado = await _mediator.Send(query);

            if (!resultado.Sucesso)
                return BadRequest(resultado);

            return await RGRResult(System.Net.HttpStatusCode.OK, resultado);
        }

        [HttpPost("filtrar")]
        public async Task<IActionResult> ObterVeiculosPaginados([FromBody] ObterVeiculosPaginadosQuery query)
        {
            return await RGRResult(System.Net.HttpStatusCode.OK, await ObterPaginado<ObterVeiculosPaginadosQuery, VeiculoDto>(query));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var veiculo = await _mediator.Send(new ObterVeiculoPorIdQuery(id));
            if (veiculo == null)
                return await RGRResult(System.Net.HttpStatusCode.NotFound, $"Veículo com ID {id} não encontrado");

            return await RGRResult(System.Net.HttpStatusCode.OK, veiculo);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] CriarVeiculoCommand command)
        {
            var id = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.Created, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(long id, [FromBody] EditarVeiculoCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id da rota diferente do Id do comando");

            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(long id)
        {
            var command = new RemoverVeiculoCommand(id);
            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }
    }
}

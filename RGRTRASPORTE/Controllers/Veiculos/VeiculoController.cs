using Application.Commands.Veiculo;
using Application.Queries.Veiculo;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Veiculos
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculoController : AbstractControllerBase
    {
        private readonly IMediator _mediator;

        public VeiculoController(
            IMediator mediator,
            INotificationContext notificationHandler)
            : base(notificationHandler)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos([FromQuery] ObterVeiculosQuery query)
        {
            var veiculos = await _mediator.Send(query);
            return await RGRResult(System.Net.HttpStatusCode.OK, veiculos);
        }

        [HttpPost("filtrar")]
        public async Task<IActionResult> ObterVeiculosPaginados([FromBody] ObterVeiculosPaginadosQuery query)
        {
            var resultado = await _mediator.Send(query);
            return await RGRResult(System.Net.HttpStatusCode.OK, resultado);
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

        [HttpPost("lote")]
        public async Task<IActionResult> AdicionarEmLote([FromBody] AdicionarVeiculosEmLoteCommand command)
        {
            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.Created, result);
        }

        [HttpPut("lote")]
        public async Task<IActionResult> EditarEmLote([FromBody] EditarVeiculosEmLoteCommand command)
        {
            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }
    }
}
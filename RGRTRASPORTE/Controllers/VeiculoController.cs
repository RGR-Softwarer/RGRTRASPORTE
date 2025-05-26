using Application.Commands.Veiculo;
using Dominio.Dtos.Veiculo;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
//Início da Funcionalidade
namespace RGRTRASPORTE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculoController : AbstractControllerBase
    {
        private readonly IMediator _mediator;

        public VeiculoController(IMediator mediator, INotificationHandler notificationHandler)
            : base(notificationHandler)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var veiculos = await _mediator.Send(new ObterTodosVeiculosQuery());
            return await RGRResult(System.Net.HttpStatusCode.OK, veiculos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var veiculo = await _mediator.Send(new ObterVeiculoPorIdQuery(id));
            if (veiculo == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, veiculo);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(VeiculoDto dto)
        {
            await _mediator.Send(new AdicionarVeiculoCommand { VeiculoDto = dto });
            return await RGRResult();
        }

        [HttpPost("Lote")]
        public async Task<IActionResult> PostEmLoteAsync(List<VeiculoDto> dto)
        {
            await _mediator.Send(new AdicionarVeiculosEmLoteCommand { VeiculosDto = dto });
            return await RGRResult();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(VeiculoDto dto)
        {
            await _mediator.Send(new EditarVeiculoCommand { VeiculoDto = dto });
            return await RGRResult();
        }

        [HttpPut("Lote")]
        public async Task<IActionResult> PutEmLoteAsync(List<VeiculoDto> dto)
        {
            await _mediator.Send(new EditarVeiculosEmLoteCommand { VeiculosDto = dto });
            return await RGRResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _mediator.Send(new RemoverVeiculoCommand(id));
            return await RGRResult();
        }

        [HttpGet("ModeloVeicular")]
        public async Task<IActionResult> ObterTodosModeloVeicular()
        {
            var modelosVeicular = await _mediator.Send(new ObterTodosModelosVeicularesQuery());
            return await RGRResult(System.Net.HttpStatusCode.OK, modelosVeicular);
        }

        [HttpGet("ModeloVeicular/{id}")]
        public async Task<IActionResult> GetByIdModeloVeicularAsync(long id)
        {
            var modeloVeicular = await _mediator.Send(new ObterModeloVeicularPorIdQuery(id));
            if (modeloVeicular == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, modeloVeicular);
        }

        [HttpPost("ModeloVeicular")]
        public async Task<IActionResult> PostModeloVeicularAsync(ModeloVeicularDto dto)
        {
            await _mediator.Send(new AdicionarModeloVeicularCommand { ModeloVeicularDto = dto });
            return await RGRResult();
        }

        [HttpPut("ModeloVeicular")]
        public async Task<IActionResult> PutModeloVeicularAsync(ModeloVeicularDto dto)
        {
            await _mediator.Send(new EditarModeloVeicularCommand { ModeloVeicularDto = dto });
            return await RGRResult();
        }

        [HttpDelete("ModeloVeicular/{id}")]
        public async Task<IActionResult> DeleteModeloVeicularAsync(long id)
        {
            await _mediator.Send(new RemoverModeloVeicularCommand(id));
            return await RGRResult();
        }
    }
}

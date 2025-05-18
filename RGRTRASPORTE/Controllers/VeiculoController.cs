using Dominio.Dtos.Veiculo;
using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Service;
using Infra.CrossCutting.Handlers.Notifications;
using Microsoft.AspNetCore.Mvc;
//Início da Funcionalidade
namespace RGRTRASPORTE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculoController : AbstractControllerBase
    {
        private readonly IVeiculoService _veiculoService;
        private readonly IModeloVeicularService _modeloVeicularService;

        public VeiculoController(IVeiculoService veiculoService, IModeloVeicularService modeloVeicularService, INotificationHandler notificationHandler) : base(notificationHandler)
        {
            _veiculoService = veiculoService;
            _modeloVeicularService = modeloVeicularService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var veiculos = await _veiculoService.ObterTodosAsync();

            return await RGRResult(System.Net.HttpStatusCode.OK, veiculos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var veiculo = await _veiculoService.ObterPorIdAsync(id);

            if (veiculo == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, veiculo);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(VeiculoDto dto)
        {
            await _veiculoService.AdicionarAsync(dto);

            return await RGRResult();
        }

        [HttpPost("Lote")]
        public async Task<IActionResult> PostEmLoteAsync(List<VeiculoDto> dto)
        {
            await _veiculoService.AdicionarEmLoteAsync(dto);

            return await RGRResult();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(VeiculoDto dto)
        {
            await _veiculoService.EditarAsync(dto);

            return await RGRResult();
        }

        [HttpPut("Lote")]
        public async Task<IActionResult> PutEmLoteAsync(List<VeiculoDto> dto)
        {
            await _veiculoService.EditarEmLoteAsync(dto);

            return await RGRResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _veiculoService.RemoverAsync(id);

            return await RGRResult();
        }

        [HttpGet("ModeloVeicular")]
        public async Task<IActionResult> ObterTodosModeloVeicular()
        {
            var modelosVeicular = await _modeloVeicularService.ObterTodosAsync();

            return await RGRResult(System.Net.HttpStatusCode.OK, modelosVeicular);
        }

        [HttpGet("ModeloVeicular/{id}")]
        public async Task<IActionResult> GetByIdModeloVeicularAsync(long id)
        {
            var modeloVeicular = await _modeloVeicularService.ObterPorIdAsync(id);

            if (modeloVeicular == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, modeloVeicular);
        }

        [HttpPost("ModeloVeicular")]
        public async Task<IActionResult> PostModeloVeicularAsync(ModeloVeicularDto dto)
        {
            await _modeloVeicularService.AdicionarAsync(dto);

            return await RGRResult();
        }

        [HttpPut("ModeloVeicular")]
        public async Task<IActionResult> PutModeloVeicularAsync(ModeloVeicularDto dto)
        {
            await _modeloVeicularService.EditarAsync(dto);

            return await RGRResult();
        }

        [HttpDelete("ModeloVeicular/{id}")]
        public async Task<IActionResult> DeleteModeloVeicularAsync(long id)
        {
            await _modeloVeicularService.RemoverAsync(id);

            return await RGRResult();
        }
    }
}

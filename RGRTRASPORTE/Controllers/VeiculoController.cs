using Dominio.Dtos;
using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Service;
using Infra.CrossCutting.Handlers.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculoController : AbstractControllerBase
    {
        private readonly IVeiculoService _veiculoService;

        public VeiculoController(IVeiculoService veiculoService, INotificationHandler notificationHandler, IUnitOfWork unitOfWork) : base(notificationHandler, unitOfWork)
        {
            _veiculoService = veiculoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var veiculos = await _veiculoService.ObterTodosAsync();

            return await RGRResult(System.Net.HttpStatusCode.OK, veiculos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
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

        [HttpPost("lote")]
        public async Task<IActionResult> PostEmLoteAsync(List<VeiculoDto> dto)
        {
            await _veiculoService.AdicionarEmLoteAsync(dto);

            return await RGRResult();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(VeiculoDto dto)
        {
            _veiculoService.EditarAsync(dto);

            return await RGRResult();
        }

        [HttpPut("lote")]
        public async Task<IActionResult> PutEmLoteAsync(List<VeiculoDto> dto)
        {
            _veiculoService.EditarEmLoteAsync(dto);

            return await RGRResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _veiculoService.RemoverAsync(id);

            return await RGRResult();
        }
    }
}

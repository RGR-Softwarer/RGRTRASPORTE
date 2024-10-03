using Dominio.Dtos.Localidades;
using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Service.Localidades;
using Infra.CrossCutting.Handlers.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Localidades
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalidadeController : AbstractControllerBase
    {
        private readonly ILocalidadeService _localidadeService;

        public LocalidadeController(ILocalidadeService localidadeService, INotificationHandler notificationHandler, IUnitOfWork unitOfWork)
            : base(notificationHandler, unitOfWork)
        {
            _localidadeService = localidadeService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var localidades = await _localidadeService.ObterTodosAsync();
            return await RGRResult(System.Net.HttpStatusCode.OK, localidades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var localidade = await _localidadeService.ObterPorIdAsync(id);
            if (localidade == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, localidade);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(LocalidadeDto dto)
        {
            await _localidadeService.AdicionarAsync(dto);
            return await RGRResult();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(LocalidadeDto dto)
        {
            await _localidadeService.EditarAsync(dto);
            return await RGRResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _localidadeService.RemoverAsync(id);
            return await RGRResult();
        }
    }
}
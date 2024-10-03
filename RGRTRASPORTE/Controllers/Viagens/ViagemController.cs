using AutoMapper;
using Dominio.Dtos.Viagens;
using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Service.Viagens;
using Infra.CrossCutting.Handlers.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Viagens
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViagemController : AbstractControllerBase
    {
        private readonly IViagemService _viagemService;

        public ViagemController(IViagemService viagemService, INotificationHandler notificationHandler, IUnitOfWork unitOfWork)
            : base(notificationHandler, unitOfWork)
        {
            _viagemService = viagemService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var viagens = await _viagemService.ObterTodosAsync();
            return await RGRResult(System.Net.HttpStatusCode.OK, viagens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var viagem = await _viagemService.ObterPorIdAsync(id);
            if (viagem == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, viagem);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(ViagemDto dto)
        {
            await _viagemService.AdicionarAsync(dto);
            return await RGRResult();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(ViagemDto dto)
        {
            await _viagemService.EditarAsync(dto);
            return await RGRResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _viagemService.RemoverAsync(id);
            return await RGRResult();
        }
    }
}
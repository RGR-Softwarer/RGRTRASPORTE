using AutoMapper;
using Dominio.Dtos.Viagens.Gatilho;
using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Service.Viagens.Gatilho;
using Infra.CrossCutting.Handlers.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Viagens.Gatilho
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatilhoViagemController : AbstractControllerBase
    {
        private readonly IGatilhoViagemService _gatinhoViagemService;

        public GatilhoViagemController(IGatilhoViagemService gatinhoViagemService, INotificationContext notificationHandler)
            : base(notificationHandler)
        {
            _gatinhoViagemService = gatinhoViagemService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var gatinhoViagens = await _gatinhoViagemService.ObterTodosAsync();
            return await RGRResult(System.Net.HttpStatusCode.OK, gatinhoViagens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var gatinhoViagem = await _gatinhoViagemService.ObterPorIdAsync(id);
            if (gatinhoViagem == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, gatinhoViagem);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(GatilhoViagemDto dto)
        {
            await _gatinhoViagemService.AdicionarAsync(dto);
            return await RGRResult();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(GatilhoViagemDto dto)
        {
            await _gatinhoViagemService.EditarAsync(dto);
            return await RGRResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _gatinhoViagemService.RemoverAsync(id);
            return await RGRResult();
        }
    }
}
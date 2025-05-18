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
    public class ViagemPassageiroController : AbstractControllerBase
    {
        private readonly IViagemPassageiroService _viagemPassageiroService;

        public ViagemPassageiroController(IViagemPassageiroService viagemPassageiroService, INotificationHandler notificationHandler)
            : base(notificationHandler)
        {
            _viagemPassageiroService = viagemPassageiroService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var viagemPassageiros = await _viagemPassageiroService.ObterTodosAsync();
            return await RGRResult(System.Net.HttpStatusCode.OK, viagemPassageiros);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var viagemPassageiro = await _viagemPassageiroService.ObterPorIdAsync(id);
            if (viagemPassageiro == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, viagemPassageiro);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(ViagemPassageiroDto dto)
        {
            await _viagemPassageiroService.AdicionarAsync(dto);
            return await RGRResult();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(ViagemPassageiroDto dto)
        {
            await _viagemPassageiroService.EditarAsync(dto);
            return await RGRResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _viagemPassageiroService.RemoverAsync(id);
            return await RGRResult();
        }
    }
}
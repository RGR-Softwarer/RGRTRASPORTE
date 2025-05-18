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
    public class ViagemPosicaoController : AbstractControllerBase
    {
        private readonly IViagemPosicaoService _viagemPosicaoService;

        public ViagemPosicaoController(IViagemPosicaoService viagemPosicaoService, INotificationHandler notificationHandler)
            : base(notificationHandler)
        {
            _viagemPosicaoService = viagemPosicaoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var viagemPosicoes = await _viagemPosicaoService.ObterTodosAsync();
            return await RGRResult(System.Net.HttpStatusCode.OK, viagemPosicoes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var viagemPosicao = await _viagemPosicaoService.ObterPorIdAsync(id);
            if (viagemPosicao == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, viagemPosicao);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(ViagemPosicaoDto dto)
        {
            await _viagemPosicaoService.AdicionarAsync(dto);
            return await RGRResult();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(ViagemPosicaoDto dto)
        {
            await _viagemPosicaoService.EditarAsync(dto);
            return await RGRResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _viagemPosicaoService.RemoverAsync(id);
            return await RGRResult();
        }
    }
}
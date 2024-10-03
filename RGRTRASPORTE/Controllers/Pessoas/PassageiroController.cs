using AutoMapper;
using Dominio.Dtos.Pessoas.Passageiros;
using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Service.Passageiros;
using Infra.CrossCutting.Handlers.Notifications;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Pessoas.Passageiros;

namespace RGRTRASPORTE.Controllers.Pessoas
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassageiroController : AbstractControllerBase
    {
        private readonly IPassageiroService _passageiroService;

        public PassageiroController(IPassageiroService passageiroService, INotificationHandler notificationHandler, IUnitOfWork unitOfWork)
            : base(notificationHandler, unitOfWork)
        {
            _passageiroService = passageiroService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var passageiros = await _passageiroService.ObterTodosAsync();
            return await RGRResult(System.Net.HttpStatusCode.OK, passageiros);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var passageiro = await _passageiroService.ObterPorIdAsync(id);
            if (passageiro == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, passageiro);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(PassageiroDto dto)
        {
            await _passageiroService.AdicionarAsync(dto);
            return await RGRResult();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(PassageiroDto dto)
        {
            await _passageiroService.EditarAsync(dto);
            return await RGRResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _passageiroService.RemoverAsync(id);
            return await RGRResult();
        }
    }
}
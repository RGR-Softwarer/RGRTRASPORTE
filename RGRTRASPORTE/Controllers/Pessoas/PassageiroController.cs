using Application.Commands.Passageiro;
using Dominio.Dtos.Pessoas.Passageiros;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Pessoas
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassageiroController : AbstractControllerBase
    {
        private readonly IMediator _mediator;

        public PassageiroController(IMediator mediator, INotificationHandler notificationHandler)
            : base(notificationHandler)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var passageiros = await _mediator.Send(new ObterTodosPassageirosQuery());
            return await RGRResult(System.Net.HttpStatusCode.OK, passageiros);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var passageiro = await _mediator.Send(new ObterPassageiroPorIdQuery(id));
            if (passageiro == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, passageiro);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(PassageiroDto dto)
        {
            await _mediator.Send(new AdicionarPassageiroCommand { PassageiroDto = dto });
            return await RGRResult();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(PassageiroDto dto)
        {
            await _mediator.Send(new EditarPassageiroCommand { PassageiroDto = dto });
            return await RGRResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _mediator.Send(new RemoverPassageiroCommand(id));
            return await RGRResult();
        }
    }
}
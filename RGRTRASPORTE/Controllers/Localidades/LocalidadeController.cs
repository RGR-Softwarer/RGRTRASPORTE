using Application.Commands.Localidade;
using Dominio.Dtos.Localidades;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Localidades
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalidadeController : AbstractControllerBase
    {
        private readonly IMediator _mediator;

        public LocalidadeController(IMediator mediator, INotificationHandler notificationHandler)
            : base(notificationHandler)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var localidades = await _mediator.Send(new ObterTodasLocalidadesQuery());
            return await RGRResult(System.Net.HttpStatusCode.OK, localidades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var localidade = await _mediator.Send(new ObterLocalidadePorIdQuery(id));
            if (localidade == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, localidade);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(LocalidadeDto dto)
        {
            await _mediator.Send(new AdicionarLocalidadeCommand { LocalidadeDto = dto });
            return await RGRResult();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(LocalidadeDto dto)
        {
            await _mediator.Send(new EditarLocalidadeCommand { LocalidadeDto = dto });
            return await RGRResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _mediator.Send(new RemoverLocalidadeCommand(id));
            return await RGRResult();
        }
    }
}
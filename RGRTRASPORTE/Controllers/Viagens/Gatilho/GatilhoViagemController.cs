using MediatR;
using Microsoft.AspNetCore.Mvc;
using Infra.CrossCutting.Handlers.Notifications;
using Application.Commands.Viagem.Gatilho;
using Application.Queries.Viagem.Gatilho;

namespace RGRTRASPORTE.Controllers.Viagens.Gatilho;

[ApiController]
[Route("api/[controller]")]
public class GatilhoViagemController : AbstractControllerBase
{
    private readonly IMediator _mediator;

    public GatilhoViagemController(
        IMediator mediator,
        INotificationContext notificationHandler)
        : base(notificationHandler)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var result = await _mediator.Send(new ObterGatilhosViagemQuery());
        return await RGRResult(System.Net.HttpStatusCode.OK, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var result = await _mediator.Send(new ObterGatilhoViagemPorIdQuery(id));
        return await RGRResult(System.Net.HttpStatusCode.OK, result);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarGatilhoViagemCommand command)
    {
        var result = await _mediator.Send(command);
        return await RGRResult(System.Net.HttpStatusCode.Created, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(long id, [FromBody] AtualizarGatilhoViagemCommand command)
    {
        //command.Id = id;
        var result = await _mediator.Send(command);
        return await RGRResult(System.Net.HttpStatusCode.OK, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(long id)
    {
        var result = await _mediator.Send(new RemoverGatilhoViagemCommand(id, "1", "1"));
        return await RGRResult(System.Net.HttpStatusCode.OK, result);
    }
}
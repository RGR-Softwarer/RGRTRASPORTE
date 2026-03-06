using Application.Queries.Dashboard;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "PassageiroOuMotorista")]
public class DashboardController : AbstractControllerBase
{
    public DashboardController(IMediator mediator, INotificationContext notificationHandler) 
        : base(notificationHandler, mediator)
    {
    }

    /// <summary>
    /// Obtém estatísticas gerais do dashboard
    /// </summary>
    [HttpGet("estatisticas")]
    public async Task<IActionResult> ObterEstatisticas(CancellationToken cancellationToken)
    {
        var query = new ObterEstatisticasDashboardQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return await RGRResult(System.Net.HttpStatusCode.OK, result);
    }

    /// <summary>
    /// Obtém viagens do dia atual
    /// </summary>
    [HttpGet("viagens-hoje")]
    public async Task<IActionResult> ObterViagensHoje(CancellationToken cancellationToken)
    {
        var query = new ObterViagensHojeQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return await RGRResult(System.Net.HttpStatusCode.OK, result);
    }

    /// <summary>
    /// Obtém veículos que estão em viagem no momento
    /// </summary>
    [HttpGet("veiculos-em-viagem")]
    public async Task<IActionResult> ObterVeiculosEmViagem(CancellationToken cancellationToken)
    {
        var query = new ObterVeiculosEmViagemQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return await RGRResult(System.Net.HttpStatusCode.OK, result);
    }
}


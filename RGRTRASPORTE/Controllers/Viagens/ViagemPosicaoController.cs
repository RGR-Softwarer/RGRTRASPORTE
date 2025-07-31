using Application.Commands.Viagem.ViagemPosicao;
using Application.Queries.Viagem.ViagemPosicao;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Viagens
{
    [Route("api/Viagem/{viagemId}/posicoes")]
    [ApiController]
    public class ViagemPosicaoController : AbstractControllerBase
    {
        private readonly IMediator _mediator;

        public ViagemPosicaoController(
            IMediator mediator,
            INotificationContext notificationHandler)
            : base(notificationHandler)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos(long viagemId, [FromQuery] ObterViagemPosicoesQuery query, CancellationToken cancellationToken)
        {
            query.ViagemId = viagemId; // Garante que o ID da rota seja usado
            var viagemPosicoes = await _mediator.Send(query, cancellationToken);
            return await RGRResult(System.Net.HttpStatusCode.OK, viagemPosicoes);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar(long viagemId, [FromBody] AdicionarViagemPosicaoApiCommand command, CancellationToken cancellationToken)
        {
            command.ViagemId = viagemId; // Garante que o ID da rota seja usado
            var id = await _mediator.Send(command, cancellationToken);
            return await RGRResult(System.Net.HttpStatusCode.Created, id);
        }
        
    }
}

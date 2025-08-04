using Application.Commands.Viagem;
using Application.Queries.Viagem;
using Hangfire;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Viagens
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViagemController : AbstractControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly string _googleApiKey;
        private readonly HttpClient _httpClient;

        public ViagemController(IMediator mediator, INotificationContext notificationHandler, IBackgroundJobClient backgroundJobClient, IConfiguration configuration) : base(notificationHandler, mediator)
        {
            _backgroundJobClient = backgroundJobClient;
            _googleApiKey = configuration["GoogleMaps:ApiKey"] ??
                string.Empty; //?? throw new InvalidOperationException("Google Maps API Key não configurada");
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos([FromQuery] ObterViagensQuery query, CancellationToken cancellationToken)
        {
            var viagens = await _mediator.Send(query, cancellationToken);
            return await RGRResult(System.Net.HttpStatusCode.OK, viagens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id, [FromQuery] bool auditado = false, CancellationToken cancellationToken = default)
        {
            var viagem = await _mediator.Send(new ObterViagemPorIdQuery(id), cancellationToken);
            if (viagem == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, viagem);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] CriarViagemCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return await RGRResult(System.Net.HttpStatusCode.Created, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(long id, [FromBody] EditarViagemCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id da rota diferente do Id do comando");

            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }

        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(long id, [FromBody] CancelarViagemCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id da rota diferente do Id do comando");

            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }

        [HttpPut("{id}/iniciar")]
        public async Task<IActionResult> Iniciar(long id, [FromBody] IniciarViagemCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id da rota diferente do Id do comando");

            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }

        [HttpPut("{id}/finalizar")]
        public async Task<IActionResult> Finalizar(long id, [FromBody] FinalizarViagemCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id da rota diferente do Id do comando");

            var result = await _mediator.Send(command);
            return await RGRResult(System.Net.HttpStatusCode.OK, result);
        }

        [HttpGet("ObterRotaViagem/{id}")]
        public async Task<IActionResult> ObterRotaViagem(long id, CancellationToken cancellationToken)
        {
            var latirudeOrigem = "-27.100"; // Latitude de Chapecó
            var logidetudaOrigem = "-52.615"; // Longitude de Chapecó
            var latirudeDestino = "-26.950"; // Latitude de Xaxim
            var logidetudaDestino = "-52.537"; // Longitude de Xaxim
            var latirudeParada = "-27.203"; // Latitude de Cordilheira Alta (parada)
            var logidetudaParada = "-52.605"; // Longitude de Cordilheira Alta (parada)

            // Construindo a URL com parada (waypoint) em Cordilheira Alta
            var url = $"https://maps.googleapis.com/maps/api/directions/json?origin={latirudeOrigem},{logidetudaOrigem}&destination={latirudeDestino},{logidetudaDestino}&waypoints={latirudeParada},{logidetudaParada}&key={_googleApiKey}";

            try
            {
                // ? MELHOR PRÁTICA: HttpClient com CancellationToken e timeout
                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(TimeSpan.FromSeconds(30)); // Timeout de 30 segundos para APIs externas

                var response = await _httpClient.GetAsync(url, timeoutCts.Token);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync(timeoutCts.Token);
                    return await RGRResult(System.Net.HttpStatusCode.OK, responseBody);
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return await RGRResult(System.Net.HttpStatusCode.RequestTimeout, "Requisição cancelada pelo cliente");
            }
            catch (OperationCanceledException)
            {
                return await RGRResult(System.Net.HttpStatusCode.RequestTimeout, "Timeout na chamada para Google Maps API");
            }
            catch (Exception ex)
            {
                return await RGRResult(System.Net.HttpStatusCode.BadRequest, $"Erro ao obter a rota da viagem: {ex.Message}");
            }

            return await RGRResult(System.Net.HttpStatusCode.BadRequest, "Erro ao obter a rota da viagem");
        }
    }
}

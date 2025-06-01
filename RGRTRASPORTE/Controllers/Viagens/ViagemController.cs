using Application.Commands.Viagem;
using Application.Queries.Viagem;
using Dominio.Interfaces.Hangfire;
using Hangfire;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Dominio.Models.Integration;
using Dominio.Models.Hangfire;
using Dominio.Dtos.Viagens;

namespace RGRTRASPORTE.Controllers.Viagens
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViagemController : AbstractControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly string _googleApiKey;
        private readonly HttpClient _httpClient;

        public ViagemController(
            IMediator mediator,
            INotificationContext notificationHandler,
            IBackgroundJobClient backgroundJobClient,
            IConfiguration configuration)
            : base(notificationHandler)
        {
            _mediator = mediator;
            _backgroundJobClient = backgroundJobClient;
            _googleApiKey = configuration["GoogleMaps:ApiKey"]??
                string.Empty; //?? throw new InvalidOperationException("Google Maps API Key não configurada");
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos([FromQuery] ObterViagensQuery query)
        {
            var viagens = await _mediator.Send(query);
            return await RGRResult(System.Net.HttpStatusCode.OK, viagens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id, [FromQuery] bool auditado = false)
        {
            var viagem = await _mediator.Send(new ObterViagemPorIdQuery(id));
            if (viagem == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, viagem);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] CriarViagemCommand command)
        {
            var id = await _mediator.Send(command);
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
        public async Task<IActionResult> ObterRotaViagem(long id)
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
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return await RGRResult(System.Net.HttpStatusCode.OK, responseBody);
                }
            }
            catch (Exception ex)
            {
                return await RGRResult(System.Net.HttpStatusCode.BadRequest, $"Erro ao obter a rota da viagem: {ex.Message}");
            }

            return await RGRResult(System.Net.HttpStatusCode.BadRequest, "Erro ao obter a rota da viagem");
        }

        [HttpGet("TesteHangifire")]
        public async Task<IActionResult> TesteHangfire()
        {
            var jobData = new Dominio.Dtos.Viagens.ViagemCriadaJobData
            {
                ViagemId = 123
            };

            _backgroundJobClient.Enqueue<IProcessadorDeEventoService>(x => x.ProcessarViagemCriada(jobData));

            return await RGRResult(System.Net.HttpStatusCode.OK, new
            {
                Success = true,
                Message = "Job agendado com sucesso"
            });
        }

        [HttpGet("TesteSilvani")]
        public async Task<IActionResult> Teste()
        {
            // 1. Configuração de exemplo
            var config = ObterConfiguracaoExemplo();

            // 2. Valores de entrada
            var valoresDinamicos = ObterValoresDinamicos();

            // 3. Token
            string token = string.Empty;
            if (config.Auth?.Type != AuthType.None)
            {
                token = await ObterTokenAsync(config.Auth);
            }

            // 4. Payload final
            var payloadFinal = GerarPayload(config.Integration.PayloadTemplate, valoresDinamicos);

            // 5. Montar request
            var request = MontarRequest(config, payloadFinal, token);

            // 6. Executar chamada
            var response = await _httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            return await RGRResult(System.Net.HttpStatusCode.OK, new
            {
                Success = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode,
                Message = response.IsSuccessStatusCode ? "Integração realizada com sucesso" : "Erro na integração",
                Response = body
            });
        }

        private IntegrationConfig ObterConfiguracaoExemplo()
        {
            return new IntegrationConfig
            {
                Auth = new AuthConfig
                {
                    Type = AuthType.Basic,
                    Credentials = new Dictionary<string, string>
            {
                { "username", "multiwebhookqa" },
                { "password", "Nz6ldbPMI0Rw4f95" }
            },
                    TokenHeaderNameToSend = "Authorization",
                    TokenHeaderFormatToSend = "Basic {token}"
                },
                Integration = new IntegrationDetails
                {
                    Url = "https://m8px22ce74.execute-api.us-east-1.amazonaws.com/tcs/webhook-gerar-carregamento",
                    Method = "POST",
                    Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            },
                    PayloadTemplate = new Dictionary<string, string>
            {
                { "tarefa", "#tarefa" },
                { "codigo", "#codigo" },
                { "volumes", "#for:volumes" }
            }
                }
            };
        }

        private Dictionary<string, object> ObterValoresDinamicos()
        {
            return new Dictionary<string, object>
    {
        { "tarefa", "carregamento123" },
        { "codigo", "456789" },
        { "volumes", new List<Dictionary<string, string>>
            {
                new() { { "codigo", "V001" }, { "quantidade", "10" } },
                new() { { "codigo", "V002" }, { "quantidade", "5" } }
            }
        }
    };
        }

        private Dictionary<string, object> GerarPayload(Dictionary<string, string> template, Dictionary<string, object> valores)
        {
            var payload = new Dictionary<string, object>();

            foreach (var campo in template)
            {
                if (campo.Value.StartsWith("#for:"))
                {
                    var caminho = campo.Value.Substring(5); // Ex: volumes
                    var nomeLista = caminho.Split('.')[0];

                    if (valores.TryGetValue(nomeLista, out var listaObj) &&
                        listaObj is List<Dictionary<string, string>> lista)
                    {
                        payload[campo.Key] = lista;
                    }
                }
                else
                {
                    var chave = campo.Value.Trim('#');
                    if (valores.TryGetValue(chave, out var valor))
                        payload[campo.Key] = valor;
                    else
                        payload[campo.Key] = campo.Value;
                }
            }

            return payload;
        }
        private HttpRequestMessage MontarRequest(IntegrationConfig config, Dictionary<string, object> payload, string token)
        {
            var jsonPayload = JsonSerializer.Serialize(payload);
            var request = new HttpRequestMessage(new HttpMethod(config.Integration.Method), config.Integration.Url)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            foreach (var header in config.Integration.Headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (!string.IsNullOrEmpty(token))
            {
                var headerName = config.Auth.TokenHeaderNameToSend ?? config.Auth.TokenHeaderName;
                var headerFormat = config.Auth.TokenHeaderFormatToSend ?? config.Auth.TokenHeaderFormat;

                var tokenHeader = headerFormat.Replace("{token}", token);
                request.Headers.TryAddWithoutValidation(headerName, tokenHeader);
            }

            return request;
        }

        private async Task<string> ObterTokenAsync(AuthConfig auth)
        {
            if (auth == null || auth.Type == AuthType.None)
                return string.Empty;

            if (auth.Type == AuthType.ApiKey)
            {
                if (auth.Credentials != null && auth.Credentials.Any())
                    return auth.Credentials.First().Value;

                throw new InvalidOperationException("Credencial de API Key não fornecida.");
            }

            if (auth.Type == AuthType.Basic)
            {
                if (!auth.Credentials.TryGetValue("username", out var user) ||
                    !auth.Credentials.TryGetValue("password", out var pass))
                {
                    throw new InvalidOperationException("Credenciais Basic incompletas.");
                }

                var bytes = Encoding.UTF8.GetBytes($"{user}:{pass}");
                return Convert.ToBase64String(bytes);
            }

            // OAuth2 e outros tipos com chamada externa
            var method = new HttpMethod(auth.AuthMethod.ToUpperInvariant());
            var request = new HttpRequestMessage(method, auth.AuthUrl)
            {
                Content = new FormUrlEncodedContent(auth.Credentials)
            };

            var response = await _httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Erro ao obter token: {response.StatusCode} - {body}");

            using var jsonDoc = JsonDocument.Parse(body);
            var tokenField = auth.TokenResponseField.Split('.');
            JsonElement current = jsonDoc.RootElement;

            foreach (var field in tokenField)
                current = current.GetProperty(field);

            return current.GetString();
        }
    }
}
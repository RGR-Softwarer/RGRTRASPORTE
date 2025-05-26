using Application.Commands.Viagem;
using Dominio.Dtos.Viagens;
using Dominio.Interfaces.Hangfire;
using Hangfire;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace RGRTRASPORTE.Controllers.Viagens
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViagemController : AbstractControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly HttpClient _httpClient = new HttpClient();

        public ViagemController(IMediator mediator, INotificationHandler notificationHandler, IBackgroundJobClient backgroundJobClient)
            : base(notificationHandler)
        {
            _mediator = mediator;
            _backgroundJobClient = backgroundJobClient;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var viagens = await _mediator.Send(new ObterTodasViagensQuery());
            return await RGRResult(System.Net.HttpStatusCode.OK, viagens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var viagem = await _mediator.Send(new ObterViagemPorIdQuery(id));
            if (viagem == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, viagem);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(ViagemDto dto)
        {
            await _mediator.Send(new AdicionarViagemCommand { ViagemDto = dto });
            return await RGRResult();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(ViagemDto dto)
        {
            await _mediator.Send(new EditarViagemCommand(dto));
            return await RGRResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _mediator.Send(new RemoverViagemCommand(id));
            return await RGRResult();
        }

        public enum AuthType
        {
            None,
            OAuth2,
            Basic,
            ApiKey
        }
        public class IntegrationConfig
        {
            public AuthConfig Auth { get; set; }
            public IntegrationDetails Integration { get; set; }
        }
        public class AuthConfig
        {
            public AuthType Type { get; set; }
            public string AuthUrl { get; set; }
            public string AuthMethod { get; set; }
            public Dictionary<string, string> Credentials { get; set; }
            public string TokenResponseField { get; set; }
            public string TokenHeaderName { get; set; }
            public string TokenHeaderFormat { get; set; }
            public string TokenHeaderNameToSend { get; set; }
            public string TokenHeaderFormatToSend { get; set; }
        }
        public class IntegrationDetails
        {
            public string Url { get; set; }
            public string Method { get; set; }
            public Dictionary<string, string> Headers { get; set; }
            public Dictionary<string, string> PayloadTemplate { get; set; }
        }

        [HttpGet("TesteHangifire")]
        public async Task<IActionResult> TesteDEV()
        {
            var jobData = new ViagemCriadaJobData
            {
                TenantId = "cliente1",
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

                throw new Exception("Credencial de API Key não fornecida.");
            }

            if (auth.Type == AuthType.Basic)
            {
                if (!auth.Credentials.TryGetValue("username", out var user) ||
                    !auth.Credentials.TryGetValue("password", out var pass))
                {
                    throw new Exception("Credenciais Basic incompletas.");
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
                throw new Exception($"Erro ao obter token: {response.StatusCode} - {body}");

            using var jsonDoc = JsonDocument.Parse(body);
            var tokenField = auth.TokenResponseField.Split('.');
            JsonElement current = jsonDoc.RootElement;

            foreach (var field in tokenField)
                current = current.GetProperty(field);

            return current.GetString();
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
            var apiKey = "AIzaSyA5KlWJ_Thsl-U6FN2TnLU8TvvHhCyPgwo"; // Substitua pela sua API Key

            // Construindo a URL com parada (waypoint) em Cordilheira Alta
            var url = $"https://maps.googleapis.com/maps/api/directions/json?origin={latirudeOrigem},{logidetudaOrigem}&destination={latirudeDestino},{logidetudaDestino}&waypoints={latirudeParada},{logidetudaParada}&key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Fazendo a requisição GET
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Verificando se a requisição foi bem-sucedida
                    if (response.IsSuccessStatusCode)
                    {
                        // Lendo o conteúdo da resposta
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return await RGRResult(System.Net.HttpStatusCode.OK, responseBody);
                    }
                    else
                    {
                        Console.WriteLine($"Erro: {response.StatusCode}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exceção: {e.Message}");
                }
            }

            return await RGRResult(System.Net.HttpStatusCode.BadRequest, "Erro ao obter a rota da viagem");
        }
    }
}
using AutoMapper;
using Dominio.Dtos.Viagens;
using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Service.Viagens;
using Infra.CrossCutting.Handlers.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers.Viagens
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViagemController : AbstractControllerBase
    {
        private readonly IViagemService _viagemService;

        public ViagemController(IViagemService viagemService, INotificationHandler notificationHandler, IUnitOfWork unitOfWork)
            : base(notificationHandler, unitOfWork)
        {
            _viagemService = viagemService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var viagens = await _viagemService.ObterTodosAsync();
            return await RGRResult(System.Net.HttpStatusCode.OK, viagens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var viagem = await _viagemService.ObterPorIdAsync(id);
            if (viagem == null)
                return NoContent();

            return await RGRResult(System.Net.HttpStatusCode.OK, viagem);
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

        [HttpPost]
        public async Task<IActionResult> PostAsync(ViagemDto dto)
        {
            await _viagemService.AdicionarAsync(dto);
            return await RGRResult();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(ViagemDto dto)
        {
            await _viagemService.EditarAsync(dto);
            return await RGRResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _viagemService.RemoverAsync(id);
            return await RGRResult();
        }
    }
}
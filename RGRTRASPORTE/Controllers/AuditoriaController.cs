using Application.Dtos;
using Dominio.Services;
using Infra.CrossCutting.Handlers.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace RGRTRASPORTE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriaController : AbstractControllerBase
    {
        private readonly IAuditoriaService _auditoriaService;

        public AuditoriaController(IAuditoriaService auditoriaService, INotificationContext notificationHandler) : base(notificationHandler)
        {
            _auditoriaService = auditoriaService;
        }

        /// <summary>
        /// Obtém o histórico completo de auditoria de uma entidade
        /// </summary>
        /// <param name="nomeEntidade">Nome da entidade (ex: Viagem, Passageiro)</param>
        /// <param name="entidadeId">ID da entidade</param>
        /// <returns>Lista de registros de auditoria</returns>
        [HttpGet("{nomeEntidade}/{entidadeId}")]
        public async Task<IActionResult> ObterHistoricoEntidade(
            string nomeEntidade, 
            long entidadeId)
        {
            try
            {
                var historico = await _auditoriaService.ObterHistoricoAsync(nomeEntidade, entidadeId);
                
                return Ok(RetornoGenericoDto<object>.ComSucesso(
                    historico.Select(r => new
                    {
                        r.Id,
                        r.NomeEntidade,
                        r.EntidadeId,
                        Acao = r.Acao.ToString(),
                        r.Usuario,
                        r.IP,
                        r.DataOcorrencia,
                        Dados = r.Dados // JSON string com os dados do evento
                    }).ToList(),
                    "Histórico de auditoria obtido com sucesso"
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(RetornoGenericoDto<object>.ComErro(
                    "Erro ao obter histórico de auditoria",
                    new List<string> { ex.Message }
                ));
            }
        }

        /// <summary>
        /// Obtém histórico de auditoria de uma viagem específica
        /// </summary>
        /// <param name="viagemId">ID da viagem</param>
        /// <returns>Histórico completo da viagem</returns>
        [HttpGet("viagem/{viagemId}")]
        public async Task<IActionResult> ObterHistoricoViagem(long viagemId)
        {
            return await ObterHistoricoEntidade("Viagem", viagemId);
        }

        /// <summary>
        /// Obtém histórico de auditoria de um passageiro específico
        /// </summary>
        /// <param name="passageiroId">ID do passageiro</param>
        /// <returns>Histórico completo do passageiro</returns>
        [HttpGet("passageiro/{passageiroId}")]
        public async Task<IActionResult> ObterHistoricoPassageiro(long passageiroId)
        {
            return await ObterHistoricoEntidade("Passageiro", passageiroId);
        }

        /// <summary>
        /// Obtém histórico de auditoria de um veículo específico
        /// </summary>
        /// <param name="veiculoId">ID do veículo</param>
        /// <returns>Histórico completo do veículo</returns>
        [HttpGet("veiculo/{veiculoId}")]
        public async Task<IActionResult> ObterHistoricoVeiculo(long veiculoId)
        {
            return await ObterHistoricoEntidade("Veiculo", veiculoId);
        }

        /// <summary>
        /// Registra um evento personalizado de auditoria
        /// </summary>
        /// <param name="request">Dados do evento personalizado</param>
        /// <returns>Confirmação do registro</returns>
        [HttpPost("evento-personalizado")]
        public async Task<IActionResult> RegistrarEventoPersonalizado(
            [FromBody] RegistrarEventoPersonalizadoRequest request)
        {
            try
            {
                await _auditoriaService.RegistrarEventoAsync(
                    request.NomeEvento,
                    request.EntidadeId,
                    request.Descricao,
                    User.Identity?.Name ?? "Sistema",
                    HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
                );

                return Ok(RetornoGenericoDto<object>.ComSucesso(
                    new { },
                    "Evento personalizado registrado com sucesso"
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(RetornoGenericoDto<object>.ComErro(
                    "Erro ao registrar evento personalizado",
                    new List<string> { ex.Message }
                ));
            }
        }
    }

    public class RegistrarEventoPersonalizadoRequest
    {
        public string NomeEvento { get; set; } = string.Empty;
        public long EntidadeId { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
} 

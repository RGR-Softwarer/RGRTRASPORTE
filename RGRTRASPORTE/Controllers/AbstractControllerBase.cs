using Application.Common;
using Application.Dtos;
using Application.Queries.Base;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace RGRTRASPORTE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class AbstractControllerBase : ControllerBase
    {
        private readonly INotificationContext _notificationHandler;
        protected readonly IMediator _mediator;

        protected AbstractControllerBase(INotificationContext notificationHandler, IMediator mediator)
        {
            _notificationHandler = notificationHandler;
            _mediator = mediator;
        }

        protected Task<ActionResult> RGRResult(HttpStatusCode statusCode = HttpStatusCode.OK, object value = null, int? pagina = null, int? limite = null, int? quantidade = null)
        {
            var existeErro = _notificationHandler.HasNotifications();

            // Se h� erro, ignora o value e retorna s� as mensagens de erro
            if (existeErro)
            {
                var resposta = new RetornoGenericoDto<object>
                {
                    Dados = null,
                    Sucesso = false,
                    Mensagem = string.Join(", ", _notificationHandler.GetNotifications().Select(x => x.Message))
                };

                return Task.FromResult<ActionResult>(new ObjectResult(resposta)
                {
                    StatusCode = (int)statusCode
                });
            }

            object dados = value;
            if (value != null)
            {
                var valueType = value.GetType();

                // Verifica se � um BaseResponse<T>
                if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(BaseResponse<>))
                {
                    var dadosProperty = valueType.GetProperty("Dados");
                    if (dadosProperty != null)
                    {
                        dados = dadosProperty.GetValue(value);
                    }
                }
            }

            var respostaSucesso = new RetornoGenericoDto<object>
            {
                Dados = dados,
                Sucesso = true,
                Mensagem = "Opera��o realizada com sucesso"
            };

            return Task.FromResult<ActionResult>(new ObjectResult(respostaSucesso)
            {
                StatusCode = (int)statusCode
            });
        }

        /// <summary>
        /// Método genérico para obter dados paginados
        /// </summary>
        /// <typeparam name="TQuery">Tipo da query paginada</typeparam>
        /// <typeparam name="TDto">Tipo do DTO de retorno</typeparam>
        /// <param name="query">Query com parâmetros de paginação</param>
        /// <returns>Resultado paginado</returns>
        protected async Task<IActionResult> ObterPaginado<TQuery, TDto>(TQuery query)
            where TQuery : QueryPaginadoBase<TDto>
        {
            var resultado = await _mediator.Send(query);

            if (!resultado.Sucesso)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        /// <summary>
        /// Método genérico para obter dados paginados com filtros
        /// </summary>
        /// <typeparam name="TQuery">Tipo da query paginada</typeparam>
        /// <typeparam name="TDto">Tipo do DTO de retorno</typeparam>
        /// <param name="filtros">Lista de filtros</param>
        /// <param name="paginaAtual">Página atual</param>
        /// <param name="tamanhoPagina">Tamanho da página</param>
        /// <param name="campoOrdenacao">Campo de ordenação</param>
        /// <param name="descendente">Se a ordenação é descendente</param>
        /// <returns>Resultado paginado</returns>
        protected async Task<IActionResult> ObterPaginado<TQuery, TDto>(
            List<FiltroGrid> filtros,
            int paginaAtual = 1,
            int tamanhoPagina = 10,
            string campoOrdenacao = "Id",
            bool descendente = false)
            where TQuery : QueryPaginadoBase<TDto>, new()
        {
            var query = new TQuery
            {
                Filtros = filtros,
                PaginaAtual = paginaAtual,
                TamanhoPagina = tamanhoPagina,
                CampoOrdenacao = campoOrdenacao,
                Descendente = descendente
            };

            return await ObterPaginado<TQuery, TDto>(query);
        }
    }
}

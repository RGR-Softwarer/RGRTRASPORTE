using Dominio.Dtos;
using Infra.CrossCutting.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace RGRTRASPORTE.Filtros
{
    public class ResultadoCustomizadoFiltro : IActionFilter, IExceptionFilter
    {
        private readonly ILogger<ResultadoCustomizadoFiltro> _logger;

        public ResultadoCustomizadoFiltro(ILogger<ResultadoCustomizadoFiltro> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NotificationException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                context.ExceptionHandled = true;
                context.Result = new JsonResult(CriarResultadoCustomizado(context));
            }
            else
            {
                _logger.LogError(context.Exception, "An error occurred: {Message}", context.Exception?.Message);
            }
        }

        private static RetornoGenericoDto CriarResultadoCustomizado(ExceptionContext contexto, object value = null)
        {
            var mensagens = new List<string>
            {
                contexto.Exception.Message
            };

            return new RetornoGenericoDto
            {
                Sucesso = true,
                Dados = value,
                Mensagens = mensagens
            };
        }
    }
}
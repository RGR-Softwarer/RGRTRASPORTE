using Dominio.Dtos;
using Dominio.Interfaces.Infra.Data;
using Infra.CrossCutting.Handlers.Notifications;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Application.Common;
using Microsoft.Extensions.Logging;
using Application.Dtos;

namespace RGRTRASPORTE.Controllers
{
    public abstract class AbstractControllerBase : ControllerBase
    {
        private readonly INotificationContext _notificationHandler;

        protected AbstractControllerBase(INotificationContext notificationHandler)
        {
            _notificationHandler = notificationHandler;
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
    }
}

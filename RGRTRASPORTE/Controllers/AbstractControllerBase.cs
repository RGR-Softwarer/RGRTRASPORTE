using Dominio.Dtos;
using Dominio.Interfaces.Infra.Data;
using Infra.CrossCutting.Handlers.Notifications;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace RGRTRASPORTE.Controllers
{
    public abstract class AbstractControllerBase : ControllerBase
    {
        private readonly INotificationHandler _notificationHandler;

        protected AbstractControllerBase(INotificationHandler notificationHandler)
        {
            _notificationHandler = notificationHandler;
        }

        protected Task<ActionResult> RGRResult(HttpStatusCode statusCode = HttpStatusCode.OK, object value = null)
        {
            var existeErro = _notificationHandler.HasNotification();

            var resposta = new RetornoGenericoDto
            {
                Dados = value,
                Mensagens = existeErro ? _notificationHandler.GetNotifications().Select(x => x.Message).ToList() : new List<string>(),
                Sucesso = !existeErro
            };

            return Task.FromResult<ActionResult>(new ObjectResult(resposta)
            {
                StatusCode = (int)statusCode
            });
        }
    }
}
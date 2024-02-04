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
        private readonly IUnitOfWork _unitOfWork;

        protected AbstractControllerBase(INotificationHandler notificationHandler, IUnitOfWork unitOfWork)
        {
            _notificationHandler = notificationHandler;
            _unitOfWork = unitOfWork;
        }

        protected async Task<ActionResult> RGRResult(HttpStatusCode statusCode = HttpStatusCode.OK, object value = null)
        {
            var existeErro = _notificationHandler.HasNotification();

            if (existeErro)
                await _unitOfWork.RollBack();
            else
                await _unitOfWork.Commit();

            var resposta = new RetornoGenericoDto
            {
                Dados = value,
                Mensagens = existeErro ? _notificationHandler.GetNotifications().Select(x => x.Message).ToList() : new List<string>(),
                Sucesso = !existeErro
            };

            return new ObjectResult(resposta)
            {
                StatusCode = (int)statusCode
            };
        }
    }
}
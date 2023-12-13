using Dominio.Dtos;
using Dominio.Interfaces.Infra.Data;
using Infra.CrossCutting.Handlers.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace PainelDeIndicadores.Controllers
{
    public abstract class AbstractControllerBase : ControllerBase
    {
        private readonly INotificationHandler _notificationHandler;
        private readonly IUnitOfWork _unitOfWork;


        public AbstractControllerBase(INotificationHandler notificationHandler, IUnitOfWork unitOfWork)
        {
            _notificationHandler = notificationHandler;
            _unitOfWork = unitOfWork;
        }

        protected ActionResult QResult(object value = null)
        {

            var existeErro = _notificationHandler.HasNotification();

            if (existeErro)
                _unitOfWork.RollBack();
            else
                _unitOfWork.Commit();


            var resposta = new RetornoGenericoDto
            {
                Dados = value,
                Mensagens = existeErro ? _notificationHandler.GetNotifications().Select(x => x.Message).ToList() : new List<string>(),
                Sucesso = !existeErro
            };


            return new ObjectResult(resposta);
        }
    }
}
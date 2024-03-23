using Infra.CrossCutting.Handlers.Notifications;

namespace Service.Bases
{
    public class NotificationBase
    {
        private readonly INotificationHandler _notificacaoHandler;

        public NotificationBase(INotificationHandler notificacaoHandler)
        {
            _notificacaoHandler = notificacaoHandler;
        }
    }
}
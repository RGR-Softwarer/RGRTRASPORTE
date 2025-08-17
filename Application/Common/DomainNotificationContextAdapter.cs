using Dominio.Interfaces;
using Infra.CrossCutting.Handlers.Notifications;

namespace Application.Common
{
    /// <summary>
    /// Adapter para converter entre INotificationContext (infraestrutura) e IDomainNotificationContext (domínio)
    /// </summary>
    internal class DomainNotificationContextAdapter : IDomainNotificationContext
    {
        private readonly INotificationContext _notificationContext;

        public DomainNotificationContextAdapter(INotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public void AddNotification(string message)
        {
            _notificationContext.AddNotification(message);
        }

        public bool HasNotifications()
        {
            return _notificationContext.HasNotifications();
        }

        public int GetNotificationCount()
        {
            return _notificationContext.GetNotifications().Count;
        }
    }
}

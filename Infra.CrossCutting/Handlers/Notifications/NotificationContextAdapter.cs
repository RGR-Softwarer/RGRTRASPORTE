using Dominio.Interfaces;

namespace Infra.CrossCutting.Handlers.Notifications
{
    public class NotificationContextAdapter : Dominio.Interfaces.INotificationContext
    {
        private readonly INotificationContext _notificationContext;

        public NotificationContextAdapter(INotificationContext notificationContext)
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

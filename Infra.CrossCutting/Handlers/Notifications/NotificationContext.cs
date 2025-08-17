using System.Net;

namespace Infra.CrossCutting.Handlers.Notifications
{
    public class NotificationContext : INotificationContext, Dominio.Interfaces.IDomainNotificationContext
    {
        private List<Notification> _notifications;

        public NotificationContext()
        {
            _notifications = new List<Notification>();
        }

        public List<Notification> GetNotifications() => _notifications;

        public void AddNotification(string message) => _notifications.Add(new Notification(message));
        public void AddNotification(string message, HttpStatusCode statusCode) => _notifications.Add(new Notification(message, statusCode));

        public void AddNotification(List<string> messages)
        {
            foreach (var message in messages)
            {
                AddNotification(message);
            }
        }

        public void AddNotification(List<string> messages, HttpStatusCode statusCode)
        {
            foreach (var message in messages)
            {
                AddNotification(message, statusCode);
            }
        }

        public bool HasNotifications() => _notifications.Count > 0;
        
        public int GetNotificationCount() => _notifications.Count;

        public void DisposeNotifications() => _notifications = new List<Notification>();
    }
}

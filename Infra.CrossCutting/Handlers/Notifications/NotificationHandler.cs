using System.Net;

namespace Infra.CrossCutting.Handlers.Notifications
{
    public class NotificationHandler : INotificationHandler
    {
        private List<Notification> _notifications;

        public NotificationHandler()
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

        public bool HasNotification() => _notifications.Any();

        public void DisposeNotifications() => _notifications = new List<Notification>();
    }
}
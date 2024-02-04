using System.Collections.Generic;
using System.Net;

namespace Infra.CrossCutting.Handlers.Notifications
{
    public interface INotificationHandler
    {
        List<Notification> GetNotifications();
        void AddNotification(string message);
        void AddNotification(string message, HttpStatusCode statusCode);
        void AddNotification(List<string> messages, HttpStatusCode statusCode);
        void AddNotification(List<string> messages);
        bool HasNotification();
        void DisposeNotifications();
    }
}

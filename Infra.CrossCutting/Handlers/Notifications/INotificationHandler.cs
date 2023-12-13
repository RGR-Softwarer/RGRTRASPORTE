using System.Collections.Generic;

namespace Infra.CrossCutting.Handlers.Notifications
{
    public interface INotificationHandler
    {
        List<Notification> GetNotifications();
        void AddNotification(string message);
        void AddNotification(List<string> messages);
        bool HasNotification();
        void DisposeNotifications();
    }
}

namespace Dominio.Interfaces
{
    public interface INotificationContext
    {
        void AddNotification(string message);
        bool HasNotifications();
        int GetNotificationCount();
    }
} 

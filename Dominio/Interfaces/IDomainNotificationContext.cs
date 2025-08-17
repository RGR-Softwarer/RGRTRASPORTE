namespace Dominio.Interfaces
{
    /// <summary>
    /// Interface para notificações de domínio - específica para DDD
    /// </summary>
    public interface IDomainNotificationContext
    {
        void AddNotification(string message);
        bool HasNotifications();
        int GetNotificationCount();
    }
}

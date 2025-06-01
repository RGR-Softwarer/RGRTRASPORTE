using Dominio.Interfaces.Infra.Data;
using Infra.CrossCutting.Handlers.Notifications;
using Microsoft.Extensions.Logging;

namespace Service.Bases
{
    public abstract class BaseService
    {
        protected readonly INotificationContext NotificationHandler;
        protected readonly ILogger Logger;
        protected readonly IUnitOfWork UnitOfWork;

        protected BaseService(
            INotificationContext notificationHandler,
            ILogger logger,
            IUnitOfWork unitOfWork)
        {
            NotificationHandler = notificationHandler;
            Logger = logger;
            UnitOfWork = unitOfWork;
        }

        protected void AddNotification(string message)
        {
            NotificationHandler.AddNotification(message);
        }

        protected void AddNotifications(IEnumerable<string> messages)
        {
            foreach (var message in messages)
            {
                AddNotification(message);
            }
        }

        protected bool IsValid()
        {
            return !NotificationHandler.HasNotifications();
        }

        protected void LogInformation(string message, params object[] args)
        {
            Logger.LogInformation(message, args);
        }

        protected void LogWarning(string message, params object[] args)
        {
            Logger.LogWarning(message, args);
        }

        protected void LogError(Exception ex, string message, params object[] args)
        {
            Logger.LogError(ex, message, args);
        }
    }
} 
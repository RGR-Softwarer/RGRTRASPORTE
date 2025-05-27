using Infra.CrossCutting.Handlers.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers
{
    public abstract class BaseHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        protected readonly INotificationHandler NotificationHandler;
        protected readonly ILogger Logger;

        protected BaseHandler(
            INotificationHandler notificationHandler,
            ILogger logger)
        {
            NotificationHandler = notificationHandler;
            Logger = logger;
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
            return !NotificationHandler.HasNotification();
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
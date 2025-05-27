using Dominio.Interfaces.Infra.Data;
using Infra.CrossCutting.Handlers.Notifications;
using Microsoft.Extensions.Logging;

namespace Service.Bases
{
    public abstract class BaseService
    {
        protected readonly INotificationHandler NotificationHandler;
        protected readonly ILogger Logger;
        protected readonly IUnitOfWork UnitOfWork;

        protected BaseService(
            INotificationHandler notificationHandler,
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

        protected async Task<bool> CommitAsync()
        {
            try
            {
                if (!IsValid())
                {
                    await UnitOfWork.RollBack();
                    return false;
                }

                await UnitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao realizar commit das alterações");
                await UnitOfWork.RollBack();
                AddNotification("Erro ao salvar as alterações");
                return false;
            }
        }
    }
} 
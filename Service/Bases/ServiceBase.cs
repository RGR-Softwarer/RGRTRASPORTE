using Infra.CrossCutting.Handlers.Notifications;
using Service.Bases;

namespace Service
{
    public class ServiceBase : NotificationBase
    {
        public ServiceBase(INotificationHandler notificationHandler) : base(notificationHandler) { }
    }
}

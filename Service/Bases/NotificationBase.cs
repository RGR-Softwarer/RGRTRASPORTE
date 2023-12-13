using FluentValidation;
using Infra.CrossCutting.Handlers.Notifications;

namespace Service.Bases
{
    public class NotificationBase
    {
        private readonly INotificationHandler _notificacaoHandler;

        public NotificationBase(INotificationHandler notificacaoHandler)
        {
            _notificacaoHandler = notificacaoHandler;
        }

        public bool Validar<TEntity>(TEntity model, AbstractValidator<TEntity> validator)
        {
            var validationresult = validator.Validate(model);

            if (!validationresult.IsValid)
                _notificacaoHandler.AddNotification(
                    validationresult.Errors.Select(x => x.ErrorMessage).ToList());

            return validationresult.IsValid;
        }
    }
}
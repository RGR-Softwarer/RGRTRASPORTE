using Dominio.Dtos.Auditoria;
using Dominio.Enums.Auditoria;
using FluentValidation;
using Infra.CrossCutting.Handlers.Notifications;
using Service.Bases;

namespace Service
{
    public class ServiceBase : NotificationBase
    {
        private readonly INotificationHandler _notificacaoHandler;

        public ServiceBase(INotificationHandler notificationHandler)
        {
            _notificacaoHandler = notificationHandler;
        }

        public bool Validar<TEntity>(TEntity model, AbstractValidator<TEntity> validator)
        {
            var validationresult = validator.Validate(model);

            if (!validationresult.IsValid)
                _notificacaoHandler.AddNotification(
                    validationresult.Errors.Select(x => x.ErrorMessage).ToList());

            return validationresult.IsValid;
        }

        public bool Validar<TEntity>(List<TEntity> model, AbstractValidator<TEntity> validator)
        {
            bool existeErro = false;

            foreach (var item in model)
            {
                var validationresult = validator.Validate(item);

                if (!validationresult.IsValid)
                {
                    _notificacaoHandler.AddNotification(
                        validationresult.Errors.Select(x => x.ErrorMessage).ToList());
                    existeErro = true;
                }
            }

            return existeErro;
        }

        public static AuditadoDto Auditado
        {
            get
            {
                return new AuditadoDto()
                {
                    OrigemAuditado = OrigemAuditadoEnum.Integradoras,
                    TipoAuditado = TipoAuditadoEnum.Sistema,
                };
            }
        }

    }
}

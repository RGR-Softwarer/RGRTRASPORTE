using Dominio.Interfaces.Infra.Data;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;

namespace Application.Behaviors
{
    public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationContext _notificationHandler;

        public UnitOfWorkBehavior(IUnitOfWork unitOfWork, INotificationContext notificationHandler)
        {
            _unitOfWork = unitOfWork;
            _notificationHandler = notificationHandler;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = await next();

                if (_notificationHandler.HasNotifications())
                    await _unitOfWork.RollBack(cancellationToken);
                else
                    await _unitOfWork.Commit(cancellationToken);

                return response;
            }
            catch
            {
                await _unitOfWork.RollBack(cancellationToken);
                throw;
            }
        }
    }
}

using Dominio.Interfaces.Infra.Data;
using Infra.CrossCutting.Handlers.Notifications;
using MediatR;

namespace Application.Behaviors
{
    public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationHandler _notificationHandler;

        public UnitOfWorkBehavior(IUnitOfWork unitOfWork, INotificationHandler notificationHandler)
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

                if (_notificationHandler.HasNotification())
                    await _unitOfWork.RollBack();
                else
                    await _unitOfWork.Commit();

                return response;
            }
            catch
            {
                await _unitOfWork.RollBack();
                throw;
            }
        }
    }
}

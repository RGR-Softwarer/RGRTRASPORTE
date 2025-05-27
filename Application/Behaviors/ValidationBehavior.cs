using FluentValidation;
using MediatR;
using Infra.CrossCutting.Handlers.Notifications;

namespace Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly INotificationHandler _notificationHandler;

        public ValidationBehavior(
            IEnumerable<IValidator<TRequest>> validators,
            INotificationHandler notificationHandler)
        {
            _validators = validators;
            _notificationHandler = notificationHandler;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                foreach (var error in failures)
                {
                    _notificationHandler.AddNotification(error.ErrorMessage);
                }
                
                return default;
            }

            return await next();
        }
    }
} 
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Infra.CrossCutting.Handlers.Notifications;

namespace Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
    private readonly INotificationContext _notificationContext;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger,
        INotificationContext notificationContext)
    {
        _validators = validators;
        _logger = logger;
        _notificationContext = notificationContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        _logger.LogInformation("Validando request {RequestType}", typeof(TRequest).Name);

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            _logger.LogWarning("Validação falhou para request {RequestType}. Erros: {@ValidationErrors}",
                typeof(TRequest).Name, failures);

            // Adiciona os erros de validação ao contexto de notificações
            foreach (var failure in failures)
            {
                _notificationContext.AddNotification(failure.ErrorMessage);
            }

            // Para a execução - deixa o controller base tratar a resposta com base nas notificações
            return default(TResponse);
        }

        return await next();
    }
} 

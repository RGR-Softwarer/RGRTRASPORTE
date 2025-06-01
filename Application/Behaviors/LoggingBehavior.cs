using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var uniqueId = Guid.NewGuid().ToString();
            var requestData = JsonSerializer.Serialize(request, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });

            _logger.LogInformation(
                "Iniciando request {RequestName} [{UniqueId}] com dados: {RequestData}", 
                requestName, 
                uniqueId,
                requestData);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                var response = await next();
                stopwatch.Stop();

                var responseData = response != null ? JsonSerializer.Serialize(response, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                }) : "null";

                _logger.LogInformation(
                    "Completado request {RequestName} [{UniqueId}] em {ElapsedMilliseconds}ms com resposta: {ResponseData}",
                    requestName,
                    uniqueId,
                    stopwatch.ElapsedMilliseconds,
                    responseData);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(
                    ex,
                    "Erro no request {RequestName} [{UniqueId}] em {ElapsedMilliseconds}ms com dados: {RequestData}",
                    requestName,
                    uniqueId,
                    stopwatch.ElapsedMilliseconds,
                    requestData);
                throw;
            }
        }
    }
} 
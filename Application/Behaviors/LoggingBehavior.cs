using MediatR;
using Serilog;
using System.Diagnostics;
using System.Text.Json;

namespace Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : class
    {
        private readonly ILogger _logger;

        public LoggingBehavior()
        {
            _logger = Log.ForContext<LoggingBehavior<TRequest, TResponse>>();
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

            _logger.Information(
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

                _logger.Information(
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
                _logger.Error(
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
using Infra.CrossCutting.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Infra.CrossCutting.Handlers.ExceptionHandler
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var errorResponse = new ErrorResponse
                {
                    Success = false,
                    Message = error.Message
                };

                switch (error)
                {
                    case BusinessException e:
                        response.StatusCode = (int)e.StatusCode;
                        errorResponse.Details = e.Message;
                        break;

                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        errorResponse.Details = "Recurso não encontrado.";
                        break;

                    default:
                        _logger.LogError(error, "Erro não tratado: {ErrorMessage}", error.Message);
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorResponse.Message = "Ocorreu um erro interno no servidor.";
                        errorResponse.Details = "Por favor, contate o suporte se o problema persistir.";
                        break;
                }

                var result = JsonSerializer.Serialize(errorResponse);
                await response.WriteAsync(result);
            }
        }
    }

    public class ErrorResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
} 
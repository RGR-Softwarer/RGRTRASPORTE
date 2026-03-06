using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Infra.CrossCutting.Multitenancy
{
    /// <summary>
    /// Middleware para validar e garantir que o header X-Tenant-Id está presente nas requisições
    /// (exceto para endpoints públicos como login e health checks)
    /// </summary>
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TenantMiddleware> _logger;
        private static readonly string[] PublicPaths = new[]
        {
            "/api/auth/login",
            "/api/auth/register",
            "/health",
            "/health/ready",
            "/health/live",
            "/swagger",
            "/api/seed"
        };

        public TenantMiddleware(RequestDelegate next, ILogger<TenantMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? string.Empty;

            // Verifica se é um endpoint público (não requer tenant)
            var isPublicPath = PublicPaths.Any(publicPath => path.StartsWith(publicPath, StringComparison.OrdinalIgnoreCase));

            if (!isPublicPath)
            {
                var tenantId = context.Request.Headers["X-Tenant-Id"].FirstOrDefault();

                // Se não tiver header X-Tenant-Id, tenta usar Host como fallback (compatibilidade)
                if (string.IsNullOrWhiteSpace(tenantId))
                {
                    tenantId = context.Request.Host.ToString();
                }

                if (string.IsNullOrWhiteSpace(tenantId))
                {
                    _logger.LogWarning("Requisição sem TenantId: {Path} de {RemoteIp}", path, context.Connection.RemoteIpAddress);
                    
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    var responseJson = JsonSerializer.Serialize(new
                    {
                        sucesso = false,
                        mensagem = "Header X-Tenant-Id é obrigatório para esta requisição"
                    });
                    await context.Response.WriteAsync(responseJson, Encoding.UTF8);
                    return;
                }

                // Armazena o tenantId no contexto para uso posterior
                context.Items["TenantId"] = tenantId;
                _logger.LogDebug("TenantId identificado: {TenantId} para path: {Path}", tenantId, path);
            }

            await _next(context);
        }
    }
}



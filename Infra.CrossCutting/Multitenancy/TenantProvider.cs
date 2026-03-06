using Microsoft.AspNetCore.Http;

namespace Infra.CrossCutting.Multitenancy
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly AsyncLocal<string> _tenantIdContext = new();

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetTenantId()
        {
            // 1. Primeiro tenta pegar do header X-Tenant-Id (preferencial)
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var tenantIdFromHeader = httpContext.Request.Headers["X-Tenant-Id"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(tenantIdFromHeader))
                {
                    return tenantIdFromHeader;
                }

                // 2. Fallback: Se não tiver header, tenta pegar do Host (compatibilidade)
                var httpTenant = httpContext.Request.Host.ToString();
                if (!string.IsNullOrWhiteSpace(httpTenant))
                {
                    return httpTenant;
                }
            }

            // 3. Se estiver em Hangfire, pega do contexto local
            if (!string.IsNullOrWhiteSpace(_tenantIdContext.Value))
                return _tenantIdContext.Value;

            throw new InvalidOperationException("TenantId não encontrado no header X-Tenant-Id, Host ou contexto.");
        }

        public string GetTenantConnectionString()
        {
            try
            {
                var tenantId = GetTenantId();

                return tenantId switch
                {
                    "localhost:5173" => "Server=66.135.11.124;Port=5432;Database=RGRTrasporte;User Id=postgres;Password=S3cureP@ssw0rd!2024;Pooling=true;MinPoolSize=1;MaxPoolSize=20;Timeout=30;CommandTimeout=60;SslMode=Prefer;",
                    "66.135.11.124:4000" => "Server=66.135.11.124;Port=5432;Database=RGRTrasporte;User Id=postgres;Password=S3cureP@ssw0rd!2024;Pooling=true;MinPoolSize=1;MaxPoolSize=20;Timeout=30;CommandTimeout=60;SslMode=Prefer;",
                    "cliente2" => "Host=localhost;Database=cliente2;Username=postgres;Password=123",
                    "localhost:5001" => "Server=84.247.182.141;Port=5432;Database=RGRTrasporte;User Id=postgres;Password=S3cureP@ssw0rd!2024;Pooling=true;MinPoolSize=1;MaxPoolSize=20;Timeout=30;CommandTimeout=60;SslMode=Prefer;",
                    "localhost:5000" => "Server=84.247.182.141;Port=5432;Database=RGRTrasporte;User Id=postgres;Password=S3cureP@ssw0rd!2024;Pooling=true;MinPoolSize=1;MaxPoolSize=20;Timeout=30;CommandTimeout=60;SslMode=Prefer;",
                    _ => "Server=84.247.182.141;Port=5432;Database=RGRTrasporte;User Id=postgres;Password=S3cureP@ssw0rd!2024;Pooling=true;MinPoolSize=1;MaxPoolSize=20;Timeout=30;CommandTimeout=60;SslMode=Prefer;" // Fallback para connection string padrão
                };
            }
            catch (InvalidOperationException)
            {
                // Se não conseguir obter o tenant ID (endpoint público), retorna connection string padrão
                return "Server=84.247.182.141;Port=5432;Database=RGRTrasporte;User Id=postgres;Password=S3cureP@ssw0rd!2024;Pooling=true;MinPoolSize=1;MaxPoolSize=20;Timeout=30;CommandTimeout=60;SslMode=Prefer;";
            }
        }

        public void SetTenantId(string tenantId)
        {
            _tenantIdContext.Value = tenantId;
        }
    }
}

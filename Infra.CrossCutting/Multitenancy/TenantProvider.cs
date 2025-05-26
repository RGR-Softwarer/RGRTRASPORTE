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

            // 1. Se estiver em HTTP, pega do header
            var httpTenant = _httpContextAccessor.HttpContext?.Request.Host.ToString();
            if (!string.IsNullOrWhiteSpace(httpTenant))
                return httpTenant;

            // 2. Se estiver em Hangfire, pega do contexto local
            if (!string.IsNullOrWhiteSpace(_tenantIdContext.Value))
                return _tenantIdContext.Value;

            throw new InvalidOperationException("TenantId não encontrado no header ou contexto.");
        }

        public string GetTenantConnectionString()
        {
            var tenantId = GetTenantId();

            return tenantId switch
            {
                "localhost:5173" => "Server=66.135.11.124;Port=5432;Database=RGRTrasporte;User Id=postgres;Password=S3cureP@ssw0rd!2024;Pooling=true;MinPoolSize=1;MaxPoolSize=20;Timeout=30;CommandTimeout=60;SslMode=Prefer;",
                "66.135.11.124:4000" => "Server=66.135.11.124;Port=5432;Database=RGRTrasporte;User Id=postgres;Password=S3cureP@ssw0rd!2024;Pooling=true;MinPoolSize=1;MaxPoolSize=20;Timeout=30;CommandTimeout=60;SslMode=Prefer;",
                "cliente2" => "Host=localhost;Database=cliente2;Username=postgres;Password=123",
                _ => throw new Exception($"Tenant '{tenantId}' não identificado.")
            };
        }

        public void SetTenantId(string tenantId)
        {
            _tenantIdContext.Value = tenantId;
        }
    }
}

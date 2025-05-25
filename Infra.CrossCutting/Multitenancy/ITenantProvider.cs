namespace Infra.CrossCutting.Multitenancy
{
    public interface ITenantProvider
    {
        string GetTenantId();
        string GetTenantConnectionString();
        void SetTenantId(string tenantId);
    }
}

using NServiceDiscovery.Configuration;

namespace NServiceDiscovery.Entity
{
    public class Tenant
    {
        public string TenantId { get; set; } = DefaultConfigurationData.DefaultTenantID;

        public string TenantToken { get; set; } = DefaultConfigurationData.DefaultTenantToken;
    }
}

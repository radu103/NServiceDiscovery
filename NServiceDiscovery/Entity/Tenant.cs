using NServiceDiscovery.Configuration;

namespace NServiceDiscovery.Entity
{
    public class Tenant
    {
        public string TenantId { get; set; } = DefaultConfigurationData.DefaultTenantID;

        public string TenantType { get; set; } = DefaultConfigurationData.DefaultTenantType;

        public string TenantToken { get; set; } = DefaultConfigurationData.DefaultTenantToken;
    }
}

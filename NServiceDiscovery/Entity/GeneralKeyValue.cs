using NServiceDiscovery.Configuration;
namespace NServiceDiscovery.Entity
{
    public class GeneralKeyValue
    {
        public string TenantId { get; set; } = DefaultConfigurationData.DefaultTenantID + "-" + DefaultConfigurationData.DefaultTenantType;

        public string Key { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }
}

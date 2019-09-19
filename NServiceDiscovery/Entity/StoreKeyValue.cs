using Newtonsoft.Json;
using NServiceDiscovery.Configuration;

namespace NServiceDiscovery.Entity
{
    public class StoreKeyValue
    {
        [JsonIgnore]
        public string TenantId { get; set; } = DefaultConfigurationData.DefaultTenantID + "-" + DefaultConfigurationData.DefaultTenantType;

        [JsonIgnore]
        public string AppName { get; set; } = string.Empty;

        [JsonProperty("key")]
        public string Key { get; set; } = string.Empty;

        [JsonProperty("value")]
        public string Value { get; set; } = string.Empty;
    }
}

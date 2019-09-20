using Newtonsoft.Json;
using System.Collections.Generic;

namespace NServiceDiscovery.Entity
{
    public class Application
    {
        [JsonIgnore]
        [JsonProperty("tenantId")]
        public string TenantId { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("protocol")]
        public ApplicationProtocol Protocol { get; set; } = ApplicationProtocol.HTTP;

        [JsonProperty("instance")]
        public List<Instance> Instances = new List<Instance>();

        [JsonProperty("configuration")]
        public List<StoreKeyValue> Configuration = new List<StoreKeyValue>();

        [JsonProperty("requiresApps")]
        public List<string> RequiresApps = new List<string>();
    }
}

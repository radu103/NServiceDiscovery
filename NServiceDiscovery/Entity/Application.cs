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

        [JsonProperty("instance")]
        public List<Instance> Instances = new List<Instance>();

        [JsonProperty("configuration")]
        public List<StoreKeyValue> Configuration = new List<StoreKeyValue>();

        [JsonProperty("dependencies")]
        public List<string> Dependencies = new List<string>();
    }
}

using Newtonsoft.Json;

namespace NServiceDiscovery.Entity
{
    public class ServiceInstanceMetadata
    {
        [JsonProperty("@class")]
        public string Class { get; set; } = "java.util.Collections$EmptyMap";
    }
}
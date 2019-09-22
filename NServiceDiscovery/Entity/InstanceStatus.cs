using Newtonsoft.Json;

namespace NServiceDiscovery.Entity
{
    public class InstanceStatus
    {
        [JsonProperty("serverInstanceId")]
        public string ServerInstanceID { get; set; }

        [JsonProperty("httpEndpoint")]
        public string HttpEndpoint { get; set; }

        [JsonProperty("secureHttpEndpoint")]
        public string SecureHttpEndpoint { get; set; }
    }
}

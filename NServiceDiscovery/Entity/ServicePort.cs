using Newtonsoft.Json;

namespace NServiceDiscovery.Entity
{
    public class ServicePort
    {
        // "@enabled" : "true"
        [JsonProperty("@enabled")]
        public bool Enabled { get; set; } = true;

        // "$" : 80
        [JsonProperty("$")]
        public int Port { get; set; } = 80;
    }
}

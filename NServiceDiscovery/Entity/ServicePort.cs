using Newtonsoft.Json;

namespace NServiceDiscovery.Entity
{
    public class ServicePort
    {
        [JsonProperty("@enabled")]
        public bool Enabled { get; set; } = true;

        [JsonProperty("$")]
        public int Port { get; set; } = 80;
    }
}

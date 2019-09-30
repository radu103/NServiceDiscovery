using Newtonsoft.Json;

namespace NServiceDiscovery.ServiceBase
{
    public class ServiceHealth
    {
        [JsonProperty("status")]
        public string Status { get; set; } = "UP";
    }
}

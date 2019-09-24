using Newtonsoft.Json;

namespace NServiceDiscovery.MQTT
{
    public class MQTTPeerMessageContent
    {
        [JsonProperty("peerId")]
        public string PeerId { get; set; } = string.Empty;

        [JsonProperty("discoveryUrls")]
        public string DiscoveryUrls { get; set; } = string.Empty;

        [JsonProperty("instanceIP")]
        public string InstanceIP { get; set; } = string.Empty;

        [JsonProperty("instancePort")]
        public int InstancePort { get; set; } = 0;
    }
}

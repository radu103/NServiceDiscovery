using Newtonsoft.Json;

namespace NServiceDiscovery.MQTT
{
    public class MQTTPeerMessageContent
    {
        [JsonProperty("peerId")]
        public string PeerId { get; set; } = string.Empty;

        [JsonProperty("discoveryUrl")]
        public string DiscoveryUrl { get; set; } = string.Empty;
    }
}

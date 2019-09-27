using Newtonsoft.Json;

namespace NServiceDiscovery.MQTT
{
    public class MQTTPersistencySyncAppsMessageContent
    {
        [JsonProperty("peerId")]
        public string PeerId { get; set; } = string.Empty;

        [JsonProperty("tenantId")]
        public string TenantId { get; set; } = string.Empty;

        [JsonProperty("appsMd5Hash")]
        public string AppsMd5Hash { get; set; } = string.Empty;
    }
}

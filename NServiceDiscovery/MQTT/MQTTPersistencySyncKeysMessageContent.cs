using Newtonsoft.Json;

namespace NServiceDiscovery.MQTT
{
    public class MQTTPersistencySyncKeysMessageContent
    {
        [JsonProperty("peerId")]
        public string PeerId { get; set; } = string.Empty;

        [JsonProperty("tenantId")]
        public string TenantId { get; set; } = string.Empty;

        [JsonProperty("keysMd5Hash")]
        public string KeysMd5Hash { get; set; } = string.Empty;
    }
}

using Newtonsoft.Json;

namespace NServiceDiscovery.MQTT
{
    public class MQTTInstanceHeartbeatMessageContent
    {
        [JsonProperty("appName")]
        public string AppName { get; set; }

        [JsonProperty("instanceId")]
        public string InstanceId { get; set; }

        [JsonProperty("tenantId")]
        public string TenantId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("lastDirtyTimestamp")]
        public long LastDirtyTimestamp { get; set; }
    }
}

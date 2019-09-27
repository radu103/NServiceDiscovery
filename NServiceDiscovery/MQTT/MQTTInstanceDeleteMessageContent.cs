using Newtonsoft.Json;

namespace NServiceDiscovery.MQTT
{
    public class MQTTInstanceDeleteMessageContent
    {
        [JsonProperty("appName")]
        public string AppName { get; set; }

        [JsonProperty("instanceId")]
        public string InstanceId { get; set; }

        [JsonProperty("tenantId")]
        public string TenantId { get; set; }
    }
}

using Newtonsoft.Json;

namespace NServiceDiscovery.MQTT
{
    public class MQTTMessage
    {
        [JsonProperty("from_instance_id")]
        public string FromInstanceId { get; set; } = string.Empty;

        [JsonProperty("to_instance_id")]
        public string ToInstanceId { get; set; } = string.Empty;

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
    }
}

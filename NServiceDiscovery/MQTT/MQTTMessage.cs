using Newtonsoft.Json;
using System.Collections.Generic;

namespace NServiceDiscovery.MQTT
{
    public class MQTTMessage
    {
        [JsonProperty("from_instance_id")]
        public string FromInstanceId { get; set; } = string.Empty;

        [JsonProperty("to_instances_ids")]
        public List<string> ToInstancesIds { get; set; } = new List<string>();

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
    }
}

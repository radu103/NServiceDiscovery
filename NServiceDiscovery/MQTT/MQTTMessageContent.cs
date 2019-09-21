
using Newtonsoft.Json;
using System;

namespace NServiceDiscovery.MQTT
{
    public class MQTTMessageContent
    {
        [JsonProperty("new_version")]
        public long NewVersion { get; set; } = 0;

        [JsonProperty("new_version_timestamp")]
        public DateTime NewVersionTimestamp { get; set; } = DateTime.MinValue;
    }
}

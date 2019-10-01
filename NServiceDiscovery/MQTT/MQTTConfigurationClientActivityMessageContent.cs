using Newtonsoft.Json;
using System;

namespace NServiceDiscovery.MQTT
{
    public class MQTTConfigurationClientActivityMessageContent
    {
        [JsonProperty("tenantId")]
        public string TenantId { get; set; }

        [JsonProperty("clientHostname")]
        public string ClientHostname { get; set; }

        [JsonProperty("lastUpdateTimestamp")]
        public DateTime LastUpdateTimestamp { get; set; }
    }
}

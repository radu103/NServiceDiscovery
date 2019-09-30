using Newtonsoft.Json;

namespace NServiceDiscovery.Common.ServiceBase
{
    public class ServiceHealth
    {
        [JsonProperty("status")]
        public string Status { get; set; } = "UP";

        [JsonProperty("peerCount")]
        public int PeerCount { get; set; } = 0;

        [JsonProperty("activeClientCount")]
        public int ActiveClientCount { get; set; } = 0;

        [JsonProperty("totalClientCount")]
        public int TotalClientCount { get; set; } = 0;

        [JsonProperty("discoveryActiveClientCount")]
        public int DiscoveryActiveClientCount { get; set; } = 0;

        [JsonProperty("configurationActiveClientCount")]
        public int ConfigurationActiveClientCount { get; set; } = 0;

        [JsonProperty("tenantCount")]
        public int TenantCount { get; set; } = 0;

        [JsonProperty("appsCount")]
        public int AppsCount { get; set; } = 0;

        [JsonProperty("appsVersion")]
        public long AppsVersion { get; set; } = 0;

        [JsonProperty("generalConfigurationKeysCount")]
        public int GeneralConfigurationKeysCount { get; set; } = 0;

        [JsonProperty("generalConfigurationVersion")]
        public long GeneralConfigurationVersion { get; set; } = 1;

        [JsonProperty("mqttMessagesSent")]
        public int MQTTMessagesSent { get; set; } = 0;

        [JsonProperty("mqttMessagesReceived")]
        public int MQTTMessagesReceived { get; set; } = 0;
    }
}

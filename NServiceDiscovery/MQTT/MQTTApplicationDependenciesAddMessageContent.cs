using Newtonsoft.Json;
using System.Collections.Generic;

namespace NServiceDiscovery.MQTT
{
    public class MQTTApplicationDependenciesAddMessageContent
    {
        [JsonProperty("appName")]
        public string AppName { get; set; }

        [JsonProperty("tenantId")]
        public string TenantId { get; set; }

        [JsonProperty("addedDependencies")]
        public List<string> AddedDependencies { get; set; }
    }
}

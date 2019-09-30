using Newtonsoft.Json;
using System.Collections.Generic;

namespace NServiceDiscovery.MQTT
{
    public class MQTTApplicationDependenciesDeleteMessageContent
    {
        [JsonProperty("appName")]
        public string AppName { get; set; }

        [JsonProperty("tenantId")]
        public string TenantId { get; set; }

        [JsonProperty("deletedDependencies")]
        public List<string> DeletedDependencies { get; set; }
    }
}

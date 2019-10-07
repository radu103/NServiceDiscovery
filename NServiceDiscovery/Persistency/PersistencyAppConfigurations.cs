using Newtonsoft.Json;
using NServiceDiscovery.Entity;
using System;
using System.Collections.Generic;

namespace NServiceDiscovery.Persistency
{
    public class PersistencyAppConfigurations : MongoRepository.Entity
    {
        [JsonProperty("tenantId")]
        public string TenantId { get; set; }

        [JsonProperty("appName")]
        public string AppName { get; set; }

        [JsonProperty("instanceId")]
        public string InstanceId { get; set; }

        [JsonProperty("utcTimestamp")]
        public DateTime UTCTimestamp { get; set; }

        [JsonProperty("versionsDelta")]
        public long VersionsDelta { get; set; }

        [JsonProperty("keyValues")]
        public List<StoreKeyValue> KeyValues { get; set; }
    }
}

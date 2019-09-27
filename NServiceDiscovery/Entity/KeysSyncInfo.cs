using Newtonsoft.Json;
using System.Collections.Generic;

namespace NServiceDiscovery.Entity
{
    public class KeysSyncInfo
    {
        [JsonProperty("tenantId")]
        public string TenantId { get; set; }

        [JsonProperty("tenantType")]
        public string TenantType { get; set; }

        [JsonProperty("keys")]
        public List<StoreKeyValue> Keys { get; set; }

        [JsonProperty("md5Hash")]
        public string MD5Hash { get; set; }
    }
}

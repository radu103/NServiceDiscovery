using Newtonsoft.Json;

namespace NServiceDiscovery.Entity
{
    public class AppsSyncInfo
    {
        [JsonProperty("tenantId")]
        public string TenantId { get; set; }

        [JsonProperty("tenantType")]
        public string TenantType { get; set; }

        [JsonProperty("apps")]
        public AllApplications Apps { get; set; }

        [JsonProperty("md5Hash")]
        public string MD5Hash { get; set; }
    }
}

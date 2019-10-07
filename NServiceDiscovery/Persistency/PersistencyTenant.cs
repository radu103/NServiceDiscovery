using Newtonsoft.Json;
using System;

namespace NServiceDiscovery.Persistency
{
    public class PersistencyTenant : MongoRepository.Entity
    {
        [JsonProperty("tenantId")]
        public string TenantId { get; set; }

        [JsonProperty("tenantToken")]
        public string TenantToken { get; set; }

        [JsonProperty("expireDate")]
        public DateTime ExpireDate { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NServiceDiscovery.Entity
{
    public class ServiceApplication
    {
        [JsonIgnore]
        [JsonProperty("tenantId")]
        public string TenantId { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("protocol")]
        public ApplicationProtocol Protocol { get; set; } = ApplicationProtocol.HTTP;

        [JsonProperty("instance")]
        public List<ServiceInstance> Instances = new List<ServiceInstance>();
    }
}

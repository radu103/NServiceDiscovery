using Newtonsoft.Json;
using NServiceDiscovery.Entity;
using System.Collections.Generic;

namespace NServiceDiscovery.RuntimeInMemory
{
    public class ServicesRuntime
    {
        [JsonIgnore]
        [JsonProperty("tenantId")]
        public string TenantId { get; set; } = string.Empty;

        [JsonProperty("versions__deltam")]
        public string VersionsDelta { get; set; } = "1";

        [JsonProperty("apps__hashcode")]
        public string AppsHashCode { get; set; } = "";

        [JsonProperty("application")]
        public static List<Application> Applications = new List<Application>();
    }
}

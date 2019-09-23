using Newtonsoft.Json;
using System.Collections.Generic;

namespace NServiceDiscovery.Entity
{
    public class CloudFoundryVcapApplication
    {
        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }

        [JsonProperty("instance_index")]
        public int InstanceIndex { get; set; }

        [JsonProperty("application_name")]
        public string ApplicationName { get; set; }

        [JsonProperty("application_uris")]
        public List<string> ApplicationUrls{ get; set; }
    }
}

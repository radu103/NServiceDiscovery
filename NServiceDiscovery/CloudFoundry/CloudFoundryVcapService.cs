using Newtonsoft.Json;
using System.Collections.Generic;

namespace NServiceDiscovery.CloudFoundry
{
    public class CloudFoundryVcapService
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("plan")]
        public string Plan { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("instance_name")]
        public string InstanceName { get; set; }

        [JsonProperty("credentials")]
        public CloudFoundryVcapServiceCredentials Credentials { get; set; }
    }
}

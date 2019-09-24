using Newtonsoft.Json;
using System.Collections.Generic;

namespace NServiceDiscovery.CloudFoundry
{
    public class CloudFoundryVcapServices
    {
        [JsonProperty("mongodb")]
        public List<CloudFoundryVcapService> MongoDBs { get; set; }

        [JsonProperty("hana")]
        public List<CloudFoundryVcapService> HanaDBs { get; set; }
    }
}

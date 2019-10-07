using Newtonsoft.Json;
using System.Collections.Generic;

namespace NServiceDiscovery.Common.CloudFoundry
{
    public class CloudFoundryVcapServices
    {
        [JsonProperty("mongodb")]
        public List<CloudFoundryVcapService> MongoDBs { get; set; }

        [JsonProperty("hana")]
        public List<CloudFoundryVcapService> HanaDBs { get; set; }

        [JsonProperty("mqtt")]
        public List<CloudFoundryVcapService> MQTTBrokers { get; set; }
    }
}

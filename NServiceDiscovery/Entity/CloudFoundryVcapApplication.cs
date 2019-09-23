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

        [JsonProperty("instance_guid", Required = Required.AllowNull)]
        public string InstanceGuid { get; set; }

        [JsonProperty("application_name")]
        public string ApplicationName { get; set; }

        [JsonProperty("application_uris")]
        public List<string> ApplicationUrls{ get; set; }

        [JsonProperty("instanceIP", Required = Required.AllowNull)]
        public string InstanceIP { get; set; }

        [JsonProperty("instancePort", Required = Required.AllowNull)]
        public int InstancePort { get; set; }
    }
}

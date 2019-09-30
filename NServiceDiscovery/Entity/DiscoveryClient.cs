using Newtonsoft.Json;
using System;

namespace NServiceDiscovery.Entity
{
    public class DiscoveryClient
    {
        [JsonProperty("clientHostname")]
        public string ClientHostname { get; set; }

        [JsonProperty("lastUpdateTimestamp")]
        public DateTime LastUpdateTimestamp { get; set; } = DateTime.UtcNow;
    }
}

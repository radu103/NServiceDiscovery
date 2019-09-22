using Newtonsoft.Json;
using System;

namespace NServiceDiscovery.Entity
{
    public class DiscoveryPeer
    {
        [JsonProperty("serverinstanceid")]
        public string ServerInstanceID { get; set; }

        [JsonProperty("lastconnecttimestamp")]
        public DateTime LastConnectTimestamp { get; set; } = DateTime.UtcNow;

        [JsonProperty("discoveryUrl")]
        public string DiscoveryUrl { get; set; }
    }
}

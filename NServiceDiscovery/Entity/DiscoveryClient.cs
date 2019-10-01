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

        public DiscoveryClient(string hostname)
        {
            ClientHostname = hostname;
            LastUpdateTimestamp = DateTime.UtcNow;
        }

        public DiscoveryClient(string hostname, DateTime utcTimestamp)
        {
            ClientHostname = hostname;
            LastUpdateTimestamp = utcTimestamp;
        }
    }
}

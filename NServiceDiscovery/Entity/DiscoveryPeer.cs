﻿using Newtonsoft.Json;
using System;

namespace NServiceDiscovery.Entity
{
    public class DiscoveryPeer
    {
        [JsonProperty("serverInstanceId")]
        public string ServerInstanceID { get; set; }

        [JsonProperty("lastUpdateTimestamp")]
        public DateTime LastUpdateTimestamp { get; set; } = DateTime.UtcNow;

        [JsonProperty("discoveryUrls")]
        public string DiscoveryUrls { get; set; }

        [JsonProperty("instanceIP", Required = Required.AllowNull)]
        public string InstanceIP { get; set; }

        [JsonProperty("instancePort", Required = Required.AllowNull)]
        public int InstancePort { get; set; }
    }
}

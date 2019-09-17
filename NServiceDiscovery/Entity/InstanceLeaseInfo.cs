using Newtonsoft.Json;
using NServiceDiscovery.Configuration;
using System;

namespace NServiceDiscovery.Entity
{
    public class InstanceLeaseInfo
    {
        [JsonProperty("renewalIntervalInSecs")]
        public int RenewalIntervalInSecs { get; set; } = DefaultConfigurationData.DefaultRenewalIntervalInSecs;

        [JsonProperty("durationInSecs")]
        public int DurationInSecs { get; set; } = DefaultConfigurationData.DefaultRenewalIntervalInSecs;

        [JsonProperty("registrationTimestamp")]
        public long RegistrationTimestamp { get; set; }

        [JsonProperty("lastRenewalTimestamp")]
        public long LastRenewalTimestamp { get; set; }

        [JsonProperty("evictionTimestamp")]
        public long EvictionTimestamp { get; set; }

        [JsonProperty("serviceUpTimestamp")]
        public long ServiceUpTimestamp { get; set; }
    }
}
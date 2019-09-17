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
        public DateTime RegistrationTimestamp { get; set; }

        [JsonProperty("lastRenewalTimestamp")]
        public DateTime LastRenewalTimestamp { get; set; }

        [JsonProperty("evictionTimestamp")]
        public DateTime EvictionTimestamp { get; set; }

        [JsonProperty("serviceUpTimestamp")]
        public DateTime ServiceUpTimestamp { get; set; }
    }
}
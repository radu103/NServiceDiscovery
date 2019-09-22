using Newtonsoft.Json;
using NServiceDiscovery.Configuration;
using System;
using NServiceDiscovery.Util;

namespace NServiceDiscovery.Entity
{
    public class InstanceLeaseInfo
    {
        [JsonProperty("renewalIntervalInSecs")]
        public int RenewalIntervalInSecs { get; set; } = DefaultConfigurationData.DefaultRenewalIntervalInSecs;

        [JsonProperty("durationInSecs")]
        public int DurationInSecs { get; set; } = DefaultConfigurationData.DefaultDurationInSecs;

        [JsonProperty("lastHealthCheckDurationInMs")]
        public int LastHealthCheckDurationInMs { get; set; } = 0;

        [JsonProperty("registrationTimestamp")]
        public long RegistrationTimestamp { get; set; } = DateTimeConversions.ToJavaMillis(DateTime.UtcNow);

        [JsonProperty("lastRenewalTimestamp")]
        public long LastRenewalTimestamp { get; set; } = DateTimeConversions.ToJavaMillis(DateTime.UtcNow);

        [JsonProperty("evictionTimestamp")]
        public long EvictionTimestamp { get; set; } = DateTimeConversions.ToJavaMillis(DateTime.UtcNow);

        [JsonProperty("serviceUpTimestamp")]
        public long ServiceUpTimestamp { get; set; } = DateTimeConversions.ToJavaMillis(DateTime.UtcNow);
    }
}
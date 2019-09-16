using System;

namespace NServiceDiscovery.Entity
{
    public class ServiceInstanceLeaseInfo
    {
        // "renewalIntervalInSecs" :  30
        public int RenewalIntervalInSecs { get; set; } = 30;

        // "durationInSecs": 90,
        public int DurationInSecs { get; set; } = 90;

        // "registrationTimestamp": 1568638937382,
        public DateTime RegistrationTimestamp { get; set; }

        // "lastRenewalTimestamp": 1568639034890,
        public DateTime LastRenewalTimestamp { get; set; }

        // "evictionTimestamp": 0,
        public DateTime EvictionTimestamp { get; set; }

        // "serviceUpTimestamp": 1568638944890
        public DateTime ServiceUpTimestamp { get; set; }
    }
}
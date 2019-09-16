using System;

namespace NServiceDiscovery.Configuration
{
    public static class DefaultConfigurationData
    {
        public static string DefaultTenantID = "public";
        public static string DefaultOwnBaseHref = "https://localhost:44334";
        public static int DefaultRenewalIntervalInSecs = 30;
        public static int DefaultDurationInSecs = 90;
        public static DateTime DefaultEvictionTimestamp = DateTime.Now.AddDays(30);
    }
}

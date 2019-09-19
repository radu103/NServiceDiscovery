using System;

namespace NServiceDiscovery.Configuration
{
    public static class DefaultConfigurationData
    {
        // default runtime constants
        public static string DefaultOwnBaseHref = "https://localhost:44334";
        public static string DefaultTenantID = "public";
        public static string DefaultTenantType = "dev";
        public static string DefaultTenantToken = "public-dev:real_token_to_be_checked";

        // instance validity
        public static int TicksPerSecond = 10000000;
        public static int DefaultRenewalIntervalInSecs = 30;
        public static int DefaultDurationInSecs = 90;
        public static int DefaultEvictionInSecs = 300;

        // persistency
        public static string TenantsConnectionString = "";
        public static string ApplicationsConnectionString = "";

        // mqtt messaging
        public static string MQTTHost = "broker.hivemq.com";
        public static int MQTTPort = 1883;
        public static string MQTTTopicName = "NServiceDiscovery-" + DefaultConfigurationData.DefaultTenantID + "-" + DefaultConfigurationData.DefaultTenantType;
    }
}

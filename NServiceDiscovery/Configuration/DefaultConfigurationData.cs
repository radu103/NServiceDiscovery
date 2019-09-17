using System;

namespace NServiceDiscovery.Configuration
{
    public static class DefaultConfigurationData
    {
        // default runtime constants
        public static string DefaultOwnBaseHref = "https://localhost:44334";
        public static string DefaultTenantID = "public";
        public static string DefaultTenantType = "dev";

        // instance validity
        public static int DefaultRenewalIntervalInSecs = 30;
        public static int DefaultDurationInSecs = 90;
        public static int DefaultEvictionInSecs = 300;

        // persistency
        public static string TenantsConnectionString = "";
        public static string ApplicationsConnectionStringTemplate = "";
    }
}

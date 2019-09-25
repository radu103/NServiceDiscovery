
namespace NServiceDiscovery.Configuration
{
    public static class DefaultConfigurationData
    {
        // default runtime constants
        public static string DefaultTenantID = "public";
        public static string DefaultTenantType = "dev";
        public static string DefaultTenantToken = "real_token_to_be_checked";

        // peer validity
        public static int DefaultEvictionTimerIntervalInSecs = 1;
        public static int DefaultPeerEvictionInSecs = 10;
        public static int DefaultPeerHeartbeatBeforeEvictionInSecs = 3;

        // instance validity
        public static int DefaultEvictionInSecs = 30;
        public static int DefaultRenewalIntervalInSecs = 30;
        public static int DefaultDurationInSecs = 90;

        // persistency config
        public static string DefaultPersistencyHostName = "";
        public static int DefaultPersistencyPort = 0;
        public static string DefaultPersistencyUsername = "";
        public static string DefaultPersistencyPassword = "";
        public static string DefaultPersistencyDBName = "";

        // mqtt messaging config
        public static string DefaultMQTTHost = "broker.hivemq.com";
        public static int DefaultMQTTPort = 1883;
        public static string DefaultMQTTUsername = string.Empty;
        public static string DefaultMQTTPassword = string.Empty;
        public static string DefaultMQTTTopicTemplate = "NServiceDiscovery-{TenantID}-{TenantType}";
        public static int DefaultMQTTReconnectSeconds = 1;
    }
}

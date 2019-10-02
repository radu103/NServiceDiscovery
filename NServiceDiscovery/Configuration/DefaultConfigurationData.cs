
namespace NServiceDiscovery.Configuration
{
    public static class DefaultConfigurationData
    {
        // default runtime constants
        public static string DefaultTenantID = "public-dev";
        public static string DefaultTenantToken = "real_token_to_be_checked";

        // client active interval validity
        public static int DefaultClientDeactivateInSecs = 60;

        // peer validity
        public static int DefaultEvictionTimerIntervalInSecs = 1;
        public static int DefaultPeerEvictionInSecs = 400;
        public static int DefaultPeerMinHeartbeatInSecs = 300;
        public static int DefaultPeerHeartbeatBeforeEvictionInSecs = 3;

        // instance validity
        public static int DefaultEvictionInSecs = 30;
        public static int DefaultRenewalIntervalInSecs = 30;
        public static int DefaultDurationInSecs = 90;

        // persistency config
        public static string DefaultPersistencyType = "MONGODB";
        public static string DefaultPersistencyHostName = "ds235711.mlab.com";
        public static int DefaultPersistencyPort = 35711;
        public static string DefaultPersistencyUsername = "admin";
        public static string DefaultPersistencyPassword = "admin2019";
        public static string DefaultPersistencyDBName = "nservicediscovery";
        public static int DefaultPersistencySyncWaitSeconds = 60;

        // persistency sync messages' constants
        //public static int DefaultPersistencySyncMinRandomSeconds = 3;
        //public static int DefaultPersistencySyncMaxRandomSeconds = 6;
        public static int DefaultPersistencySyncMinRandomSeconds = 300;
        public static int DefaultPersistencySyncMaxRandomSeconds = 600;

        // mqtt messaging config
        public static string DefaultMQTTHost = "broker.hivemq.com";
        public static int DefaultMQTTPort = 1883;
        public static string DefaultMQTTUsername = string.Empty;
        public static string DefaultMQTTPassword = string.Empty;
        public static string DefaultMQTTTopicTemplate = "NServiceDiscovery-{TenantId}";
        public static int DefaultMQTTReconnectSeconds = 1;
    }
}

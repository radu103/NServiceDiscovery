
namespace NServiceDiscovery
{
    public class ConfigurationData
    {
        public string ServerInstanceID { get; set; }
        public string Urls { get; set; }

        public int EvictionTimerIntervalInSecs { get; set; }
        public int PeerEvictionInSecs { get; set; }
        public int PeerMinHeartbeatInSecs { get; set; }
        public int PeerHeartbeatBeforeEvictionInSecs { get; set; }

        public int EvictionInSecs { get; set; }
        public int RenewalIntervalInSecs { get; set; }
        public int DurationInSecs { get; set; }

        public string PersistencyType { get; set; }
        public string PersistencyHostName { get; set; }
        public int PersistencyPort { get; set; }
        public string PersistencyUsername { get; set; }
        public string PersistencyPassword { get; set; }
        public string PersistencyDBName { get; set; }
        public int PersistencySyncWaitSeconds { get; set; }
        public int PersistencySyncMinRandomSeconds { get; set; }
        public int PersistencySyncMaxRandomSeconds { get; set; }

        public string MQTTHost { get; set; }
        public int MQTTPort { get; set; }
        public string MQTTUsername { get; set; }
        public string MQTTPassword { get; set; }
        public int MQTTReconnectSeconds { get; set; }
        public string MQTTTopicTemplate { get; set; }
    }
}

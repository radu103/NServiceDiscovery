using System;

namespace NServiceDiscovery
{
    public class ConfigurationData
    {
        public String ServerInstanceID { get; set; }

        public String TenantID { get; set; }
        public String TenantType { get; set; }
        public String TenantToken { get; set; }

        public int EvictionInSecs { get; set; }
        public int RenewalIntervalInSecs { get; set; }
        public int DurationInSecs { get; set; }

        public string PersistencyHostName { get; set; }
        public int PersistencyPort { get; set; }
        public string PersistencyUsername { get; set; }
        public string PersistencyPassword { get; set; }
        public string PersistencyDBName { get; set; }

        public string MQTTHost { get; set; }
        public int MQTTPort { get; set; }
        public string MQTTUsername { get; set; }
        public string MQTTPassword { get; set; }
        public string MQTTTopicName { get; set; }
        public int MQTTReconnectSeconds { get; set; }
    }
}

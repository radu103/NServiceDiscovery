using NServiceDiscovery.Configuration;
using System;

namespace NServiceDiscovery
{
    public class ConfigurationHelper
    {
        public String TenantID { get; set; }

        public static ConfigurationData Load()
        {
            var conf = new ConfigurationData(){

                ServerInstanceID = Guid.NewGuid().ToString(),

                TenantID = DefaultConfigurationData.DefaultTenantID,
                TenantType = DefaultConfigurationData.DefaultTenantType,
                TenantToken = DefaultConfigurationData.DefaultTenantToken,

                EvictionTimerIntervalInSecs = DefaultConfigurationData.DefaultEvictionTimerIntervalInSecs,
                PeerEvictionInSecs = DefaultConfigurationData.DefaultPeerEvictionInSecs,

                EvictionInSecs = DefaultConfigurationData.DefaultEvictionInSecs,
                RenewalIntervalInSecs = DefaultConfigurationData.DefaultRenewalIntervalInSecs,
                DurationInSecs = DefaultConfigurationData.DefaultDurationInSecs,

                PersistencyHostName = DefaultConfigurationData.DefaultPersistencyHostName,
                PersistencyPort = DefaultConfigurationData.DefaultPersistencyPort,
                PersistencyUsername = DefaultConfigurationData.DefaultPersistencyUsername,
                PersistencyPassword = DefaultConfigurationData.DefaultPersistencyPassword,
                PersistencyDBName = DefaultConfigurationData.DefaultPersistencyDBName,

                MQTTHost = DefaultConfigurationData.DefaultMQTTHost,
                MQTTPort = DefaultConfigurationData.DefaultMQTTPort,
                MQTTUsername = DefaultConfigurationData.DefaultMQTTUsername,
                MQTTPassword = DefaultConfigurationData.DefaultMQTTPassword,
                MQTTTopicName = DefaultConfigurationData.DefaultMQTTTopicName,
                MQTTReconnectSeconds = DefaultConfigurationData.DefaultMQTTReconnectSeconds
            };

            return conf;
        }
    }
}

using NServiceDiscovery.CloudFoundry;
using NServiceDiscovery.Configuration;
using System;

namespace NServiceDiscovery
{
    public class ConfigurationHelper
    {
        public static ConfigurationData Load(CloudFoundryVcapApplication cfApp, string instanceGuid = "")
        {

            var conf = new ConfigurationData(){

                ServerInstanceID = Guid.NewGuid().ToString(),

                EvictionTimerIntervalInSecs = DefaultConfigurationData.DefaultEvictionTimerIntervalInSecs,
                PeerEvictionInSecs = DefaultConfigurationData.DefaultPeerEvictionInSecs,
                PeerMinHeartbeatInSecs = DefaultConfigurationData.DefaultPeerMinHeartbeatInSecs,
                PeerHeartbeatBeforeEvictionInSecs = DefaultConfigurationData.DefaultPeerHeartbeatBeforeEvictionInSecs,

                EvictionInSecs = DefaultConfigurationData.DefaultEvictionInSecs,
                RenewalIntervalInSecs = DefaultConfigurationData.DefaultRenewalIntervalInSecs,
                DurationInSecs = DefaultConfigurationData.DefaultDurationInSecs,

                PersistencyType = DefaultConfigurationData.DefaultPersistencyType,
                PersistencyHostName = DefaultConfigurationData.DefaultPersistencyHostName,
                PersistencyPort = DefaultConfigurationData.DefaultPersistencyPort,
                PersistencyUsername = DefaultConfigurationData.DefaultPersistencyUsername,
                PersistencyPassword = DefaultConfigurationData.DefaultPersistencyPassword,
                PersistencyDBName = DefaultConfigurationData.DefaultPersistencyDBName,
                PersistencySyncWaitSeconds = DefaultConfigurationData.DefaultPersistencySyncWaitSeconds,
                PersistencySyncMinRandomSeconds = DefaultConfigurationData.DefaultPersistencySyncMinRandomSeconds,
                PersistencySyncMaxRandomSeconds = DefaultConfigurationData.DefaultPersistencySyncMaxRandomSeconds,

                MQTTHost = DefaultConfigurationData.DefaultMQTTHost,
                MQTTPort = DefaultConfigurationData.DefaultMQTTPort,
                MQTTUsername = DefaultConfigurationData.DefaultMQTTUsername,
                MQTTPassword = DefaultConfigurationData.DefaultMQTTPassword,
                MQTTReconnectSeconds = DefaultConfigurationData.DefaultMQTTReconnectSeconds
            };

            if (cfApp != null)
            {
                if (string.IsNullOrEmpty(instanceGuid))
                {
                    conf.ServerInstanceID = cfApp.ApplicationId + "-" + cfApp.InstanceIndex;
                }
                else
                {
                    conf.ServerInstanceID = instanceGuid;
                }
            }

            return conf;
        }
    }
}

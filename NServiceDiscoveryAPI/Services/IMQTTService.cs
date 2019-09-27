using NServiceDiscovery.MQTT;
using System.Collections.Generic;

namespace NServiceDiscoveryAPI.Services
{
    public interface IMQTTService
    {
        void SendMQTTMessageToAll(string tenantId, MQTTMessage message);

        void SendMQTTMessageToInstance(string tenantId, string toInstanceId, MQTTMessage message);

        void SendMQTTMessageToMultipleInstances(string tenantId, List<string> toInstanceIds, MQTTMessage message);
    }
}
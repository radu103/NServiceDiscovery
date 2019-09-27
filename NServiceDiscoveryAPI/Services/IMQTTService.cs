using NServiceDiscovery.MQTT;

namespace NServiceDiscoveryAPI.Services
{
    public interface IMQTTService
    {
        void sendMQTTMessageToAll(string tenantId, string tenantType, MQTTMessage message);

        void sendMQTTMessageToInstance(string tenantId, string tenantType, string toInstanceId, MQTTMessage message);
    }
}
using NServiceDiscovery.MQTT;

namespace NServiceDiscoveryAPI.Services
{
    public interface IMQTTProcessingService
    {
        string GetTenantFromTopicName(string topic);
        void ProcessClientConfigurationActivity(MQTTMessage mqttMessage, string topic);
        void ProcessClientDiscoveryActivity(MQTTMessage mqttMessage, string topic);
        void ProcessInstanceAdded(MQTTMessage mqttMessage, string topic);
        void ProcessInstanceChangeStatus(MQTTMessage mqttMessage, string topic);
        void ProcessInstanceDelete(MQTTMessage mqttMessage, string topic);
        void ProcessInstanceHeartbeat(string topic, MQTTMessage mqttMessage);
        void ProcessInstanceUpdated(MQTTMessage mqttMessage, string topic);
    }
}
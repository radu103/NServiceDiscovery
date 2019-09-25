using MQTTnet.Client;

namespace NServiceDiscoveryAPI.MQTT
{
    internal class MyMQTTClient
    {
        public string TenantId { get; set; }

        public string TenantType { get; set; }

        public string mqttTopic { get; set; }

        public IMqttClient mqttClient { get; set; }
    }
}

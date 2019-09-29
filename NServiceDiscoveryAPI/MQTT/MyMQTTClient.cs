using MQTTnet.Client;
using MQTTnet.Protocol;

namespace NServiceDiscoveryAPI.MQTT
{
    internal class MyMQTTClient
    {
        public string TenantId { get; set; }

        public string TenantType { get; set; }

        public string mqttTopic { get; set; }

        public string mqttClientId { get; set; }

        public IMqttClient mqttClient { get; set; }

        public static MqttQualityOfServiceLevel MQTTQualityOfService = MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce;
    }
}

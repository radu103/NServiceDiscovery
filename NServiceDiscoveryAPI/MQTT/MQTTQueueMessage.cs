using MQTTnet.Protocol;

namespace NServiceDiscoveryAPI.MQTT
{
    public class MQTTQueueMessage
    {
        public string Topic { get; set; }

        public string QueuedMessage { get; set; }

        public MqttQualityOfServiceLevel QoS { get; set; }
    }
}

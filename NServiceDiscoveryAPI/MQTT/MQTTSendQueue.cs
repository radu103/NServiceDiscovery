using System.Collections.Generic;

namespace NServiceDiscoveryAPI.MQTT
{
    public class MQTTSendQueue
    {
        public static List<MQTTQueueMessage> MessagesToSend = new List<MQTTQueueMessage>();

        public static bool CheckIfMQTTIsConfigured()
        {
            var isConfigured = false;

            isConfigured = !string.IsNullOrEmpty(Program.mqttSettings.Host) && !string.IsNullOrEmpty(Program.mqttSettings.TopicTemplate);

            return isConfigured;
        }
    }
}

using System.Collections.Generic;

namespace NServiceDiscoveryAPI.MQTT
{
    public class MQTTSendQueue
    {
        public static List<MQTTQueueMessage> MessagesToSend = new List<MQTTQueueMessage>();

        public static bool CheckIfMQTTIsConfigured()
        {
            var isConfigured = false;

            isConfigured = !string.IsNullOrEmpty(Program.InstanceConfig.MQTTHost) && !string.IsNullOrEmpty(Program.InstanceConfig.MQTTTopicTemplate);

            return isConfigured;
        }
    }
}

using System.Collections.Generic;

namespace NServiceDiscoveryAPI.MQTT
{
    public class MQTTSendQueue
    {
        public static List<MQTTQueueMessage> MessagesToSend = new List<MQTTQueueMessage>();
    }
}

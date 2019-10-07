namespace NServiceDiscovery.MQTT
{
    public class MQTTSettings : IMQTTSettings
    {
        public string Host { get; set; } = "broker.hivemq.com";

        public int Port { get; set; } = 1883;

        public string User { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string TopicTemplate { get; set; } = "NServiceDiscovery-{TenantId}";

        public int ReconnectSeconds { get; set; } = 1;
    }
}

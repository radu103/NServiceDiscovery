namespace NServiceDiscovery.MQTT
{
    public interface IMQTTSettings
    {
        string Host { get; set; }
        string Password { get; set; }
        int Port { get; set; }
        string TopicTemplate { get; set; }
        string User { get; set; }
        int ReconnectSeconds { get; set; }
    }
}
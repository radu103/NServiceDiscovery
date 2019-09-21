using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using Newtonsoft.Json;
using NServiceDiscovery.Configuration;
using NServiceDiscovery.MQTT;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NServiceDiscoveryAPI.Services
{
    public class MQTTService : IMQTTService
    {
        private MqttFactory _factory;
        private IMqttClient _mqttClient;
        private string _topic;

        public MQTTService()
        {
            _factory = new MqttFactory();
            _mqttClient = _factory.CreateMqttClient();
            _topic = DefaultConfigurationData.MQTTTopicName;

            var options = new MqttClientOptionsBuilder()
                    .WithTcpServer(DefaultConfigurationData.MQTTHost, DefaultConfigurationData.MQTTPort)
                    .Build();

            _mqttClient.UseDisconnectedHandler(async disconnected =>
            {
                Console.WriteLine("DISCONNECTED FROM MQTT BROKER");
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    await _mqttClient.ConnectAsync(options, CancellationToken.None);
                }
                catch(Exception err)
                {
                    Console.WriteLine(err.Message);
                    Console.WriteLine("RECONNECTING FAILED");
                }
            });

            _mqttClient.UseConnectedHandler(async conn =>
            {
                Console.WriteLine("CONNECTED TO MQTT BROKER WITH CLIENT ID : '" + conn.AuthenticateResult.AssignedClientIdentifier + "'");

                _mqttClient.SubscribeAsync(_topic);
            });

            _mqttClient.UseApplicationMessageReceivedHandler(async message =>
            {
                var jsonStr = message.ApplicationMessage.ConvertPayloadToString();

                Console.WriteLine("MQTT MESSAGE RECEIVED");
                Console.WriteLine(jsonStr);
                Console.WriteLine(string.Empty);

                var mqttMessage = JsonConvert.DeserializeObject<MQTTMessage>(jsonStr);

                Console.WriteLine("MQTT MESSAGE RECEIVED");
                Console.WriteLine(jsonStr);
                Console.WriteLine(string.Empty);
            });

            var task = _mqttClient.ConnectAsync(options);
            task.Wait();
        }

    }
}

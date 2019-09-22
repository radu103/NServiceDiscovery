using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;
using NServiceDiscovery.MQTT;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NServiceDiscoveryAPI.Services
{
    public class MQTTService : IMQTTService
    {
        private MqttFactory _factory = new MqttFactory();

        private IMqttClient _mqttClient;
        private IMqttClientOptions _mqttClientOptions;
        private string _mqttTopic = string.Empty;

        public MQTTService()
        {
            _mqttClient = _factory.CreateMqttClient();
            _mqttTopic = Program.InstanceConfig.MQTTTopicName;

            if (string.IsNullOrEmpty(Program.InstanceConfig.MQTTUsername))
            {
                _mqttClientOptions = new MqttClientOptionsBuilder()
                                .WithTcpServer(Program.InstanceConfig.MQTTHost, Program.InstanceConfig.MQTTPort)
                                .WithClientId(Program.InstanceConfig.ServerInstanceID)
                                .Build();
            }
            else
            {
                _mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(Program.InstanceConfig.MQTTHost, Program.InstanceConfig.MQTTPort)
                    .WithCredentials(Program.InstanceConfig.MQTTUsername, Program.InstanceConfig.MQTTPassword)
                    .WithClientId(Program.InstanceConfig.ServerInstanceID)
                    .Build();
            }

            _mqttClient.UseDisconnectedHandler(async disconnected =>
            {
                Console.WriteLine("MQTT CLIENT - DISCONNECTED FROM MQTT BROKER");
                await Task.Delay(TimeSpan.FromSeconds(Program.InstanceConfig.MQTTReconnectSeconds));

                try
                {
                    await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
                }
                catch(Exception err)
                {
                    Console.WriteLine(err.Message);
                    Console.WriteLine("MQTT CLIENT - RECONNECTING FAILED");
                }
            });

            _mqttClient.UseConnectedHandler(async conn =>
            {
                Console.WriteLine("MQTT CLIENT - CONNECTED TO MQTT BROKER WITH CLIENT ID : '" + conn.AuthenticateResult.AssignedClientIdentifier + "'");

                _mqttClient.SubscribeAsync(_mqttTopic);
            });

            _mqttClient.UseApplicationMessageReceivedHandler(async message =>
            {
                var jsonStr = message.ApplicationMessage.ConvertPayloadToString();

                Console.WriteLine("MQTT CLIENT - MQTT MESSAGE RECEIVED");
                Console.WriteLine(jsonStr);
                Console.WriteLine(string.Empty);

                var mqttMessage = JsonConvert.DeserializeObject<MQTTMessage>(jsonStr);

                Console.WriteLine("MQTT CLIENT - MQTT MESSAGE RECEIVED");
                Console.WriteLine(jsonStr);
                Console.WriteLine(string.Empty);
            });

            var task = _mqttClient.ConnectAsync(_mqttClientOptions);
            task.Wait();
        }

    }
}

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;
using NServiceDiscovery.MQTT;
using NServiceDiscovery.RuntimeInMemory;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using NServiceDiscovery.Entity;

namespace NServiceDiscoveryAPI.Services
{
    public class MQTTService : IMQTTService
    {
        private MqttFactory _factory = new MqttFactory();

        private IMqttClient _mqttClient;
        private IMqttClientOptions _mqttClientOptions;

        private string _mqttClientID = string.Empty;
        private string _mqttTopic = string.Empty;

        public MQTTService()
        {
            _mqttClient = _factory.CreateMqttClient();

            _mqttClientID = Program.InstanceConfig.ServerInstanceID;
            _mqttTopic = Program.InstanceConfig.MQTTTopicName;

            if (string.IsNullOrEmpty(Program.InstanceConfig.MQTTUsername))
            {
                _mqttClientOptions = new MqttClientOptionsBuilder()
                                .WithTcpServer(Program.InstanceConfig.MQTTHost, Program.InstanceConfig.MQTTPort)
                                .WithClientId(_mqttClientID)
                                .Build();
            }
            else
            {
                _mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(Program.InstanceConfig.MQTTHost, Program.InstanceConfig.MQTTPort)
                    .WithCredentials(Program.InstanceConfig.MQTTUsername, Program.InstanceConfig.MQTTPassword)
                    .WithClientId(_mqttClientID)
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

                // subscribe to tenant and landscape specific topic
                await _mqttClient.SubscribeAsync(_mqttTopic);

                // create my peer data object
                var myPeerData = new MQTTPeerMessageContent()
                {
                    PeerId = _mqttClientID,
                    DiscoveryUrl = string.Empty
                };

                var jsonPeer = JsonConvert.SerializeObject(myPeerData).Replace("\"", "'");

                // send mqtt peer message
                _mqttClient.PublishAsync(_mqttTopic, "{\"from_instance_id\":\"id1\",\"to_instance_id\":\"ALL\",\"type\":\"INSTANCE_CONNECTED\",\"message\":\"" + jsonPeer + "\"}");
            });

            _mqttClient.UseApplicationMessageReceivedHandler(async message =>
            {
                var jsonStr = message.ApplicationMessage.ConvertPayloadToString();

                Console.WriteLine("MQTT CLIENT - MQTT MESSAGE RECEIVED");
                Console.WriteLine(jsonStr);
                Console.WriteLine(string.Empty);

                var mqttMessage = JsonConvert.DeserializeObject<MQTTMessage>(jsonStr);

                if (mqttMessage.ToInstanceId.CompareTo("ALL") == 0){

                    if(mqttMessage.Type.CompareTo("INSTANCE_CONNECTED") == 0)
                    {
                        var receivedMessage = mqttMessage.Message.ToString().Replace("'", "\"");

                        var peerMessageContent = JsonConvert.DeserializeObject<MQTTPeerMessageContent>(receivedMessage);

                        if (peerMessageContent != null && peerMessageContent.PeerId.CompareTo(_mqttClientID) != 0)
                        {
                            var existingPeer = Memory.Peers.SingleOrDefault(p => p.ServerInstanceID.CompareTo(peerMessageContent.PeerId) == 0);

                            if (existingPeer == null)
                            {
                                var newPeer = new DiscoveryPeer()
                                {
                                    LastConnectTimestamp = DateTime.UtcNow,
                                    ServerInstanceID = peerMessageContent.PeerId,
                                    DiscoveryUrl = peerMessageContent.DiscoveryUrl
                                };

                                Memory.Peers.Add(newPeer);
                            }
                        }
                    }
                }

                Console.WriteLine("MQTT CLIENT - MQTT MESSAGE RECEIVED");
                Console.WriteLine(jsonStr);
                Console.WriteLine(string.Empty);
            });

            var task = _mqttClient.ConnectAsync(_mqttClientOptions);
            task.Wait();
        }

    }
}

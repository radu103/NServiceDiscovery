﻿using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;
using NServiceDiscovery.MQTT;
using NServiceDiscovery.RuntimeInMemory;
using System;
using System.Threading.Tasks;
using System.Linq;
using NServiceDiscovery.Entity;
using System.Timers;
using System.Threading;
using NServiceDiscovery.Repository;
using System.Collections.Generic;
using NServiceDiscoveryAPI.MQTT;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Connecting;

namespace NServiceDiscoveryAPI.Services
{
    public class MQTTService : IMQTTService
    {
        private IMemoryDiscoveryPeerRepository _memoryDiscoveryPeerRepository;

        private MqttFactory _factory = new MqttFactory();
        private string _mqttClientID { get; set; }
        private List<MyMQTTClient> _mqttClients = new List<MyMQTTClient>();
        private IMqttClientOptions _mqttClientOptions;

        private System.Timers.Timer _broadcastPeerTimer;

        private async void UseDisconnectedHandler(IMqttClient mqttClient, MqttClientDisconnectedEventArgs disconnected)
        {
            Console.WriteLine("MQTT CLIENT - DISCONNECTED FROM MQTT BROKER");
            await Task.Delay(TimeSpan.FromSeconds(Program.InstanceConfig.MQTTReconnectSeconds));

            try
            {
                await mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                Console.WriteLine("MQTT CLIENT - RECONNECTING FAILED");
            }
        }

        private async void UseConnectedHandler(IMqttClient mqttClient, string topic, MqttClientConnectedEventArgs conn)
        {
            Console.WriteLine("MQTT CLIENT - CONNECTED TO MQTT BROKER");

            // subscribe to tenant and landscape specific topic
            mqttClient.SubscribeAsync(topic, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce);

            // brodcast my peer info
            BroadcastMyPeerInfo("INSTANCE_CONNECTED");
        }

        private async void UseApplicationMessageReceivedHandler(MqttApplicationMessageReceivedEventArgs message)
        {
            var jsonStr = message.ApplicationMessage.ConvertPayloadToString();

            Console.WriteLine("MQTT CLIENT - MQTT MESSAGE RECEIVED");
            Console.WriteLine(jsonStr);
            Console.WriteLine(string.Empty);

            var mqttMessage = JsonConvert.DeserializeObject<MQTTMessage>(jsonStr);

            // do not process own messages except ALL broadcast messages
            if (mqttMessage != null && (mqttMessage.FromInstanceId.CompareTo(_mqttClientID) != 0 || mqttMessage.ToInstancesIds.IndexOf("ALL") == 0))
            {
                // process peer broadcast message
                if (mqttMessage.ToInstancesIds.IndexOf("ALL") >= 0 && mqttMessage.Type.CompareTo("INSTANCE_CONNECTED") == 0 && mqttMessage.FromInstanceId.CompareTo(_mqttClientID) != 0)
                {
                    ProcessInstanceConnected(message.ApplicationMessage.Topic, mqttMessage);
                }

                // TO DO : other messages
                if (mqttMessage.ToInstancesIds.IndexOf("ALL") >= 0 && mqttMessage.Type.CompareTo("INSTANCE_HEARTBEAT") == 0 && mqttMessage.FromInstanceId.CompareTo(_mqttClientID) != 0)
                {
                    ProcessInstanceHeartbeat(message.ApplicationMessage.Topic, mqttMessage);
                }

                // TO DO : other messages
            }

            Console.WriteLine("MQTT CLIENT - MQTT MESSAGE RECEIVED");
            Console.WriteLine(jsonStr);
            Console.WriteLine(string.Empty);
        }

        public MQTTService(IMemoryDiscoveryPeerRepository memoryDiscoveryPeerRepository)
        {
            _memoryDiscoveryPeerRepository = memoryDiscoveryPeerRepository;

            _mqttClientID = Program.InstanceConfig.ServerInstanceID;

            // single tenant case, only 1 MQTT client needed
            if (!string.IsNullOrEmpty(Program.SINGLE_TENANT_ID))
            {
                var mqttClient = _factory.CreateMqttClient();

                var myMqttClient = new MyMQTTClient()
                {
                    TenantId = Program.SINGLE_TENANT_ID,
                    TenantType = Program.SINGLE_TENANT_TYPE,
                    mqttTopic = Program.InstanceConfig.MQTTTopicName,
                    mqttClient = mqttClient
                };
               

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

                mqttClient.UseDisconnectedHandler(async disconnected =>
                {
                    UseDisconnectedHandler(mqttClient, disconnected);
                });

                mqttClient.UseConnectedHandler(async conn =>
                {
                    UseConnectedHandler(mqttClient, myMqttClient.mqttTopic, conn);
                });

                mqttClient.UseApplicationMessageReceivedHandler(async message =>
                {
                    UseApplicationMessageReceivedHandler(message);
                });

                var task = mqttClient.ConnectAsync(_mqttClientOptions);
                task.Wait();

                _mqttClients.Add(myMqttClient);
            }

            // start broadcast timer
            _broadcastPeerTimer = new System.Timers.Timer(Math.Min(Program.InstanceConfig.PeerEvictionInSecs - Program.InstanceConfig.PeerHeartbeatBeforeEvictionInSecs, 5) * 1000);
            _broadcastPeerTimer.AutoReset = true;
            _broadcastPeerTimer.Enabled = true;
            _broadcastPeerTimer.Elapsed += OnBroadcastTimedEvent;
        }

        private void OnBroadcastTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Send broadcast to Peers at {0:HH:mm:ss.fff}", e.SignalTime);
            BroadcastMyPeerInfo();
        }

        private MQTTMessage GetMyPeerMessage(string toInstanceId = "ALL", string type = "INSTANCE_CONNECTED")
        {
            var myPeerData = new MQTTPeerMessageContent()
            {
                PeerId = _mqttClientID,
                DiscoveryUrls = string.Format("http://{0}/eureka/apps", Program.InstanceConfig.Urls),
                InstanceIP = Program.INSTANCE_IP,
                InstancePort = Program.INSTANCE_PORT
            };

            var jsonPeer = JsonConvert.SerializeObject(myPeerData).Replace("\"", "'");

            var destInstances = new List<string>();
            destInstances.Add(toInstanceId);

            var newPeerMessage = new MQTTMessage()
            {
                FromInstanceId = _mqttClientID,
                ToInstancesIds = destInstances,
                Type = type,
                Message = jsonPeer
            };

            return newPeerMessage;
        }

        private void BroadcastMyPeerInfo(string type = "INSTANCE_HEARTBEAT")
        {
            var myPeerMessage = GetMyPeerMessage("ALL", type);
            for(var i = 0; i < _mqttClients.Count; i++)
            {
                if (_mqttClients[i].mqttClient.IsConnected)
                {
                    _mqttClients[i].mqttClient.PublishAsync(_mqttClients[i].mqttTopic, JsonConvert.SerializeObject(myPeerMessage));
                }
            }
        }

        private void ProcessInstanceConnected(string topic, MQTTMessage mqttMessage)
        {
            var receivedMessage = mqttMessage.Message.ToString().Replace("'", "\"");

            var peerMessageContent = JsonConvert.DeserializeObject<MQTTPeerMessageContent>(receivedMessage);

            if (peerMessageContent != null && peerMessageContent.PeerId.CompareTo(_mqttClientID) != 0)
            {
                // respond back with my peer data
                var myMqttClient = _mqttClients.SingleOrDefault(c => c.mqttTopic.CompareTo(topic) == 0);

                if (myMqttClient != null && myMqttClient.mqttClient.IsConnected)
                {
                    var myPeerMessage = GetMyPeerMessage(peerMessageContent.PeerId);
                    myMqttClient.mqttClient.PublishAsync(myMqttClient.mqttTopic, JsonConvert.SerializeObject(myPeerMessage));
                }

                // add new peer to my peers
                var existingPeer = Memory.Peers.SingleOrDefault(p => p.ServerInstanceID.CompareTo(peerMessageContent.PeerId) == 0);

                if (existingPeer == null)
                {
                    var newPeer = new DiscoveryPeer()
                    {
                        LastUpdateTimestamp = DateTime.UtcNow,
                        ServerInstanceID = peerMessageContent.PeerId,
                        DiscoveryUrls = peerMessageContent.DiscoveryUrls,
                        InstanceIP = peerMessageContent.InstanceIP,
                        InstancePort = peerMessageContent.InstancePort
                    };

                    Memory.Peers.Add(newPeer);
                }
            }
        }

        private void ProcessInstanceHeartbeat(string topic, MQTTMessage mqttMessage)
        {
            var receivedMessage = mqttMessage.Message.ToString().Replace("'", "\"");

            var peerMessageContent = JsonConvert.DeserializeObject<MQTTPeerMessageContent>(receivedMessage);

            if (peerMessageContent != null && peerMessageContent.PeerId.CompareTo(_mqttClientID) != 0)
            {
                // add new peer to my peers
                var existingPeer = Memory.Peers.SingleOrDefault(p => p.ServerInstanceID.CompareTo(peerMessageContent.PeerId) == 0);

                if (existingPeer == null)
                {
                    var newPeer = new DiscoveryPeer()
                    {
                        LastUpdateTimestamp = DateTime.UtcNow,
                        ServerInstanceID = peerMessageContent.PeerId,
                        DiscoveryUrls = peerMessageContent.DiscoveryUrls,
                        InstanceIP = peerMessageContent.InstanceIP,
                        InstancePort = peerMessageContent.InstancePort
                    };

                    _memoryDiscoveryPeerRepository.Add(newPeer);
                }
                else
                {
                    existingPeer.LastUpdateTimestamp = DateTime.UtcNow;
                    existingPeer.DiscoveryUrls = peerMessageContent.DiscoveryUrls;
                    existingPeer.InstanceIP = peerMessageContent.InstanceIP;
                    existingPeer.InstancePort = peerMessageContent.InstancePort;
                }
            }
        }
    }
}

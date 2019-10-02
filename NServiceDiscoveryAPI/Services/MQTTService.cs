using MQTTnet;
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
using MQTTnet.Client.Publishing;

namespace NServiceDiscoveryAPI.Services
{
    public class MQTTService : IMQTTService
    {
        private IMQTTProcessingService _mqttProcessingService;

        private MqttFactory _factory = new MqttFactory();
        private List<MyMQTTClient> _mqttClients = new List<MyMQTTClient>();
        private IMqttClientOptions _mqttClientOptions;

        private System.Timers.Timer _broadcastPeerTimer;

        public MQTTService(IMQTTProcessingService mqttProcessingService)
        {
            _mqttProcessingService = mqttProcessingService;

            // no mqtt messaging if no MQTT broker
            if (string.IsNullOrEmpty(Program.InstanceConfig.MQTTHost))
            {
                return;
            }

            // when single tenant case, only 1 MQTT client needed
            for (var t = 0; t < Program.Tenants.Count; t++)
            {
                var mqttTopic = Program.InstanceConfig.MQTTTopicTemplate.Replace("{TenantId}", Program.Tenants[t].TenantId);
                var mqttClient = _factory.CreateMqttClient();

                var mqttClientID = Program.InstanceConfig.ServerInstanceID + ":" + Program.Tenants[t].TenantId;

                var myMqttClient = new MyMQTTClient()
                {
                    TenantId = Program.Tenants[t].TenantId,
                    mqttTopic = mqttTopic,
                    mqttClient = mqttClient,
                    mqttClientId = mqttClientID
                };

                if (string.IsNullOrEmpty(Program.InstanceConfig.MQTTUsername))
                {
                    _mqttClientOptions = new MqttClientOptionsBuilder()
                                    .WithTcpServer(Program.InstanceConfig.MQTTHost, Program.InstanceConfig.MQTTPort)
                                    .WithClientId(myMqttClient.mqttClientId)
                                    .Build();
                }
                else
                {
                    _mqttClientOptions = new MqttClientOptionsBuilder()
                        .WithTcpServer(Program.InstanceConfig.MQTTHost, Program.InstanceConfig.MQTTPort)
                        .WithCredentials(Program.InstanceConfig.MQTTUsername, Program.InstanceConfig.MQTTPassword)
                        .WithClientId(myMqttClient.mqttClientId)
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

                mqttClient.ConnectAsync(_mqttClientOptions);

                _mqttClients.Add(myMqttClient);
            }

            // start broadcast timer
            _broadcastPeerTimer = new System.Timers.Timer(Math.Min(Program.InstanceConfig.PeerEvictionInSecs - Program.InstanceConfig.PeerHeartbeatBeforeEvictionInSecs, Program.InstanceConfig.PeerMinHeartbeatInSecs) * 1000);
            _broadcastPeerTimer.AutoReset = true;
            _broadcastPeerTimer.Enabled = true;
            _broadcastPeerTimer.Elapsed += OnBroadcastTimedEvent;
        }

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

            // send topic queue messages (topic is specific to tenantId and mqttClient already
            if(MQTTSendQueue.MessagesToSend.Count > 0)
            {
                lock (MQTTSendQueue.MessagesToSend)
                {
                    // ignore if it changes
                    try
                    {
                        var qMessages = MQTTSendQueue.MessagesToSend.FindAll(qm => qm.Topic.CompareTo(topic) == 0).ToList();

                        foreach (var qMessage in MQTTSendQueue.MessagesToSend)
                        {
                            if (mqttClient.IsConnected)
                            {
                                mqttClient.PublishAsync(topic, qMessage.QueuedMessage, qMessage.QoS);
                                MQTTSendQueue.MessagesToSend.Remove(qMessage);
                                InstanceHealthService.Health.MQTTMessagesSent += 1;
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        private async void UseApplicationMessageReceivedHandler(MqttApplicationMessageReceivedEventArgs message)
        {
            var jsonStr = message.ApplicationMessage.ConvertPayloadToString();

            //Console.WriteLine("MQTT CLIENT - MQTT MESSAGE RECEIVED");
            //Console.WriteLine(jsonStr);
            //Console.WriteLine(string.Empty);

            var mqttMessage = JsonConvert.DeserializeObject<MQTTMessage>(jsonStr);

            // do not process own messages except ALL broadcast messages
            if (mqttMessage != null && (mqttMessage.FromInstanceId.IndexOf(Program.InstanceConfig.ServerInstanceID) < 0 || mqttMessage.ToInstancesIds.IndexOf("ALL") == 0))
            {
                // process peer broadcast message : INSTANCE_CONNECTED
                if (mqttMessage.ToInstancesIds.IndexOf("ALL") >= 0 && mqttMessage.Type.CompareTo("INSTANCE_CONNECTED") == 0 && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
                {
                    ProcessInstanceConnected(message.ApplicationMessage.Topic, mqttMessage);
                }

                // process peer broadcast message : INSTANCE_HEARTBEAT
                if (mqttMessage.ToInstancesIds.IndexOf("ALL") >= 0 && mqttMessage.Type.CompareTo("INSTANCE_HEARTBEAT") == 0 && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
                {
                    _mqttProcessingService.ProcessInstanceHeartbeat(message.ApplicationMessage.Topic, mqttMessage);
                }

                // TO DO : other broadcast messages

                // process peer message : ADD_INSTANCE or UPDATE_INSTANCE
                if (mqttMessage.ToInstancesIds.IndexOf(Program.InstanceConfig.ServerInstanceID) >= 0 && (mqttMessage.Type.CompareTo("ADD_INSTANCE") == 0 || mqttMessage.Type.CompareTo("UPDATE_INSTANCE") == 0) && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
                {
                    _mqttProcessingService.ProcessInstanceAdded(mqttMessage, message.ApplicationMessage.Topic);
                }

                if (mqttMessage.ToInstancesIds.IndexOf(Program.InstanceConfig.ServerInstanceID) >= 0 && (mqttMessage.Type.CompareTo("UPDATE_INSTANCE") == 0 || mqttMessage.Type.CompareTo("UPDATE_INSTANCE") == 0) && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
                {
                    _mqttProcessingService.ProcessInstanceUpdated(mqttMessage, message.ApplicationMessage.Topic);
                }

                // process peer message : CHANGE_INSTANCE_STATUS
                if (mqttMessage.ToInstancesIds.IndexOf(Program.InstanceConfig.ServerInstanceID) >= 0 && mqttMessage.Type.CompareTo("CHANGE_INSTANCE_STATUS") == 0 && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
                {
                    _mqttProcessingService.ProcessInstanceChangeStatus(mqttMessage, message.ApplicationMessage.Topic);
                }

                // process peer message : DELETE_INSTANCE
                if (mqttMessage.ToInstancesIds.IndexOf(Program.InstanceConfig.ServerInstanceID) >= 0 && mqttMessage.Type.CompareTo("DELETE_INSTANCE") == 0 && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
                {
                    _mqttProcessingService.ProcessInstanceDelete(mqttMessage, message.ApplicationMessage.Topic);
                }

                // process peer message : CLIENT_DISCOVERY_ACTIVITY
                if (mqttMessage.ToInstancesIds.IndexOf(Program.InstanceConfig.ServerInstanceID) >= 0 && mqttMessage.Type.CompareTo("CLIENT_DISCOVERY_ACTIVITY") == 0 && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
                {
                    _mqttProcessingService.ProcessClientDiscoveryActivity(mqttMessage, message.ApplicationMessage.Topic);
                }

                // process peer message : CLIENT_CONFIGURATION_ACTIVITY
                if (mqttMessage.ToInstancesIds.IndexOf(Program.InstanceConfig.ServerInstanceID) >= 0 && mqttMessage.Type.CompareTo("CLIENT_CONFIGURATION_ACTIVITY") == 0 && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
                {
                    _mqttProcessingService.ProcessClientConfigurationActivity(mqttMessage, message.ApplicationMessage.Topic);
                }

                // TO DO : other peer to peer messages
            }
        }

        private void OnBroadcastTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Send broadcast to Peers at {0:HH:mm:ss.fff}", e.SignalTime);
            BroadcastMyPeerInfo();
        }

        private MQTTMessage GetPeerMessage(string toInstanceId = "ALL", string type = "INSTANCE_CONNECTED")
        {
            var myPeerData = new MQTTPeerMessageContent()
            {
                PeerId = Program.InstanceConfig.ServerInstanceID,
                DiscoveryUrls = string.Format("http://{0}/eureka/apps", Program.InstanceConfig.Urls),
                InstanceIP = Program.INSTANCE_IP,
                InstancePort = Program.INSTANCE_PORT
            };

            var jsonPeer = JsonConvert.SerializeObject(myPeerData).Replace("\"", "'");

            var destInstances = new List<string>();
            destInstances.Add(toInstanceId);

            var newPeerMessage = new MQTTMessage()
            {
                FromInstanceId = Program.InstanceConfig.ServerInstanceID,
                ToInstancesIds = destInstances,
                Type = type,
                Message = jsonPeer
            };

            return newPeerMessage;
        }

        private void BroadcastMyPeerInfo(string type = "INSTANCE_HEARTBEAT")
        {
            try
            {
                var myPeerMessage = GetPeerMessage("ALL", type);

                for (var t = 0; t < _mqttClients.Count; t++)
                {
                    var topicName = _mqttClients[t].mqttTopic;
                    var jsonMessage = JsonConvert.SerializeObject(myPeerMessage);

                    while (!_mqttClients[t].mqttClient.IsConnected)
                    {
                        Thread.Sleep(10);
                    }

                    if (_mqttClients[t].mqttClient.IsConnected)
                    {
                        _mqttClients[t].mqttClient.PublishAsync(topicName, jsonMessage, MyMQTTClient.MQTTQualityOfService);
                        InstanceHealthService.Health.MQTTMessagesSent += 1;
                    }
                    else
                    {
                        if (MQTTSendQueue.CheckIfMQTTIsConfigured())
                        {
                            MQTTSendQueue.MessagesToSend.Add(new MQTTQueueMessage()
                            {
                                Topic = topicName,
                                QueuedMessage = jsonMessage,
                                QoS = MyMQTTClient.MQTTQualityOfService
                            });
                        }
                    }
                }
            }
            catch(Exception err)
            {
                Console.WriteLine("BroadcastMyPeerInfo ERROR : ", err.Message);
                Console.WriteLine("BroadcastMyPeerInfo Stack Trace : ", err.StackTrace);
            }
        }

        /*  SEND OR QUEUE MESSAGES  */

        public void SendMQTTMessageToAll(string tenantId, MQTTMessage message)
        {
            Task task = Task.Factory.StartNew(async () => {

                MqttClientPublishResult res = null;

                var myMqttClient = _mqttClients.SingleOrDefault(c => (c.TenantId + "-" + c.TenantType).CompareTo(tenantId) == 0);

                string jsonMessage = JsonConvert.SerializeObject(message);

                if (myMqttClient != null && myMqttClient.mqttClient.IsConnected)
                {
                    res = await myMqttClient.mqttClient.PublishAsync(myMqttClient.mqttTopic, jsonMessage, MyMQTTClient.MQTTQualityOfService);
                    InstanceHealthService.Health.MQTTMessagesSent += 1;
                }
                else
                {
                    if (MQTTSendQueue.CheckIfMQTTIsConfigured())
                    {
                        MQTTSendQueue.MessagesToSend.Add(new MQTTQueueMessage()
                        {
                            Topic = myMqttClient.mqttTopic,
                            QueuedMessage = jsonMessage,
                            QoS = MyMQTTClient.MQTTQualityOfService
                        });
                    }
                }

                return res;
            });
        }

        public void SendMQTTMessageToInstance(string tenantId, string toInstanceId, MQTTMessage message)
        {
            Task task = Task.Factory.StartNew(async () =>
            {
                MqttClientPublishResult res = null;

                var myMqttClient = _mqttClients.SingleOrDefault(c => (c.TenantId + "-" + c.TenantType).CompareTo(tenantId) == 0);

                message.ToInstancesIds = new List<string>();
                message.ToInstancesIds.Add(toInstanceId);

                string jsonMessage = JsonConvert.SerializeObject(message);

                if (myMqttClient != null && myMqttClient.mqttClient.IsConnected)
                {
                    res = await myMqttClient.mqttClient.PublishAsync(myMqttClient.mqttTopic, jsonMessage, MyMQTTClient.MQTTQualityOfService);
                    InstanceHealthService.Health.MQTTMessagesSent += 1;
                }
                else
                {
                    if (MQTTSendQueue.CheckIfMQTTIsConfigured())
                    {
                        MQTTSendQueue.MessagesToSend.Add(new MQTTQueueMessage()
                        {
                            Topic = myMqttClient.mqttTopic,
                            QueuedMessage = jsonMessage,
                            QoS = MyMQTTClient.MQTTQualityOfService
                        });
                    }
                }

                return res;
            });
        }

        public void SendMQTTMessageToMultipleInstances(string tenantId, List<string> toInstanceIds, MQTTMessage message)
        {
            Task task = Task.Factory.StartNew(async () =>
            {
                MqttClientPublishResult res = null;

                var myMqttClient = _mqttClients.SingleOrDefault(c => (c.TenantId + "-" + c.TenantType).CompareTo(tenantId) == 0);

                message.ToInstancesIds = toInstanceIds;

                string jsonMessage = JsonConvert.SerializeObject(message);

                if (myMqttClient != null && myMqttClient.mqttClient.IsConnected)
                {
                    res = await myMqttClient.mqttClient.PublishAsync(myMqttClient.mqttTopic, jsonMessage, MyMQTTClient.MQTTQualityOfService);
                    InstanceHealthService.Health.MQTTMessagesSent += 1;
                }
                else
                {
                    if (MQTTSendQueue.CheckIfMQTTIsConfigured())
                    {
                        MQTTSendQueue.MessagesToSend.Add(new MQTTQueueMessage()
                        {
                            Topic = myMqttClient.mqttTopic,
                            QueuedMessage = jsonMessage,
                            QoS = MyMQTTClient.MQTTQualityOfService
                        });
                    }
                }

                return res;
            });
        }

        /*  PROCESS RECEIVING MESSAGES  */

        private void ProcessInstanceConnected(string topic, MQTTMessage mqttMessage)
        {
            InstanceHealthService.Health.MQTTMessagesReceived += 1;

            var receivedMessage = mqttMessage.Message.ToString().Replace("'", "\"");

            var peerMessageContent = JsonConvert.DeserializeObject<MQTTPeerMessageContent>(receivedMessage);

            if (peerMessageContent != null && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
            {
                // respond back with my peer data
                var myMqttClient = _mqttClients.SingleOrDefault(c => c.mqttTopic.CompareTo(topic) == 0);

                if (myMqttClient != null && myMqttClient.mqttClient.IsConnected)
                {
                    var myPeerMessage = GetPeerMessage(peerMessageContent.PeerId, "INSTANCE_HEARTBEAT");
                    myMqttClient.mqttClient.PublishAsync(myMqttClient.mqttTopic, JsonConvert.SerializeObject(myPeerMessage));
                }

                // add new peer to my peers
                DiscoveryPeer existingPeer = null;

                lock (Memory.Peers)
                {
                    existingPeer = Memory.Peers.FirstOrDefault(p => p.ServerInstanceID.CompareTo(peerMessageContent.PeerId) == 0);
                }

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
    }
}

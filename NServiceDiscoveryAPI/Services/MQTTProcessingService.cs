using Newtonsoft.Json;
using NServiceDiscovery.Entity;
using NServiceDiscovery.MQTT;
using NServiceDiscovery.Repository;
using NServiceDiscovery.RuntimeInMemory;
using System;
using System.Linq;

namespace NServiceDiscoveryAPI.Services
{
    public class MQTTProcessingService : IMQTTProcessingService
    {
        public IMemoryDiscoveryPeerRepository _memoryDiscoveryPeerRepository;
        public IMemoryDiscoveryClientRepository _clientDiscoveryRepo;
        public IMemoryGeneralConfigurationClientRepository _clientConfigurationRepo;

        public MQTTProcessingService(IMemoryDiscoveryPeerRepository memoryDiscoveryPeerRepository, IMemoryDiscoveryClientRepository clientDiscoveryRepo, IMemoryGeneralConfigurationClientRepository clientConfigurationRepo)
        {
            _memoryDiscoveryPeerRepository = memoryDiscoveryPeerRepository;
            _clientDiscoveryRepo = clientDiscoveryRepo;
            _clientConfigurationRepo = clientConfigurationRepo;
        }
        
        public string GetTenantFromTopicName(string topic)
        {
            var aux = Program.mqttSettings.TopicTemplate.Split(new char[] { '-' });
            var tenantIdType = topic.Replace(aux[0] + "-", string.Empty);
            return tenantIdType;
        }

        public void ProcessInstanceHeartbeat(string topic, MQTTMessage mqttMessage)
        {
            InstanceHealthService.Health.MQTTMessagesReceived += 1;

            var receivedMessage = mqttMessage.Message.ToString().Replace("'", "\"");

            var peerMessageContent = JsonConvert.DeserializeObject<MQTTPeerMessageContent>(receivedMessage);

            if (peerMessageContent != null && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
            {
                DiscoveryPeer existingPeer = null;

                // add new peer to my peers
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

        public void ProcessInstanceAdded(MQTTMessage mqttMessage, string topic)
        {
            InstanceHealthService.Health.MQTTMessagesReceived += 1;

            var receivedMessage = mqttMessage.Message.ToString().Replace("'", "\"");

            var instance = JsonConvert.DeserializeObject<Instance>(receivedMessage);
            instance.TenantId = GetTenantFromTopicName(topic);

            if (instance != null && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
            {
                var memoryRepo = new MemoryServicesRepository(instance.TenantId, Program.InstanceConfig.EvictionInSecs);
                memoryRepo.Add(instance);
            }
        }

        public void ProcessInstanceUpdated(MQTTMessage mqttMessage, string topic)
        {
            InstanceHealthService.Health.MQTTMessagesReceived += 1;

            var receivedMessage = mqttMessage.Message.ToString().Replace("'", "\"");

            var instance = JsonConvert.DeserializeObject<Instance>(receivedMessage);
            instance.TenantId = GetTenantFromTopicName(topic);

            if (instance != null && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
            {
                var memoryRepo = new MemoryServicesRepository(instance.TenantId, Program.InstanceConfig.EvictionInSecs);
                memoryRepo.Add(instance);
            }
        }

        public void ProcessInstanceChangeStatus(MQTTMessage mqttMessage, string topic)
        {
            InstanceHealthService.Health.MQTTMessagesReceived += 1;

            var receivedMessage = mqttMessage.Message.ToString().Replace("'", "\"");

            var messageContent = JsonConvert.DeserializeObject<MQTTInstanceChangeStatusMessageContent>(receivedMessage);

            if (messageContent != null && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
            {
                var memoryRepo = new MemoryServicesRepository(messageContent.TenantId, Program.InstanceConfig.EvictionInSecs);
                memoryRepo.ChangeStatus(messageContent.AppName, messageContent.InstanceId, messageContent.Status, messageContent.LastDirtyTimestamp);
            }
        }

        public void ProcessInstanceDelete(MQTTMessage mqttMessage, string topic)
        {
            InstanceHealthService.Health.MQTTMessagesReceived += 1;

            var receivedMessage = mqttMessage.Message.ToString().Replace("'", "\"");

            var messageContent = JsonConvert.DeserializeObject<MQTTInstanceDeleteMessageContent>(receivedMessage);

            if (messageContent != null && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
            {
                var memoryRepo = new MemoryServicesRepository(messageContent.TenantId, Program.InstanceConfig.EvictionInSecs);
                memoryRepo.Delete(messageContent.AppName, messageContent.InstanceId);
            }
        }

        public void ProcessClientDiscoveryActivity(MQTTMessage mqttMessage, string topic)
        {
            InstanceHealthService.Health.MQTTMessagesReceived += 1;

            var receivedMessage = mqttMessage.Message.ToString().Replace("'", "\"");

            var messageContent = JsonConvert.DeserializeObject<MQTTDiscoveryClientActivityMessageContent>(receivedMessage);

            if (messageContent != null && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
            {
                this._clientDiscoveryRepo.Add(new DiscoveryClient(messageContent.ClientHostname, messageContent.LastUpdateTimestamp));
            }
        }

        public void ProcessClientConfigurationActivity(MQTTMessage mqttMessage, string topic)
        {
            InstanceHealthService.Health.MQTTMessagesReceived += 1;

            var receivedMessage = mqttMessage.Message.ToString().Replace("'", "\"");

            var messageContent = JsonConvert.DeserializeObject<MQTTConfigurationClientActivityMessageContent>(receivedMessage);

            if (messageContent != null && mqttMessage.FromInstanceId.CompareTo(Program.InstanceConfig.ServerInstanceID) != 0)
            {
                this._clientConfigurationRepo.Add(new DiscoveryClient(messageContent.ClientHostname, messageContent.LastUpdateTimestamp));
            }
        }
    }
}

using Newtonsoft.Json;
using NServiceDiscovery.MQTT;
using NServiceDiscovery.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NServiceDiscoveryAPI.Services
{
    public class PublishClientsService : IPublishClientsService
    {
        private IMQTTService _mqttService;
        private IMemoryDiscoveryPeerRepository _discoveryPeerRepository;

        public PublishClientsService(IMQTTService mqttService, IMemoryDiscoveryPeerRepository discoveryPeerRepository)
        {
            _mqttService = mqttService;
            _discoveryPeerRepository = discoveryPeerRepository;
        }

        private List<string> GetPeerIds()
        {
            var peers = _discoveryPeerRepository.GetAll();
            var toIds = peers.Select(p => p.ServerInstanceID).Distinct().ToList();

            return toIds;
        }

        public void PublishClientDiscoveryActivity(string tenantId, string clientHostname)
        {
            var toIds = GetPeerIds();

            if (toIds.Count == 0)
            {
                return;
            }

            var activityMessage = new MQTTDiscoveryClientActivityMessageContent()
            {
                TenantId = tenantId,
                ClientHostname = clientHostname,
                LastUpdateTimestamp = DateTime.UtcNow
            };

            var jsonMessage = JsonConvert.SerializeObject(activityMessage);
            jsonMessage = jsonMessage.Replace("\"", "'");

            var mqttMessage = new MQTTMessage()
            {
                FromInstanceId = Program.InstanceConfig.ServerInstanceID,
                ToInstancesIds = toIds,
                Type = "CLIENT_DISCOVERY_ACTIVITY",
                Message = jsonMessage
            };

            _mqttService.SendMQTTMessageToMultipleInstances(tenantId, toIds, mqttMessage);
        }

        public void PublishClientConfigurationActivity(string tenantId, string clientHostname)
        {
            var toIds = GetPeerIds();

            if (toIds.Count == 0)
            {
                return;
            }

            var activityMessage = new MQTTConfigurationClientActivityMessageContent()
            {
                TenantId = tenantId,
                ClientHostname = clientHostname,
                LastUpdateTimestamp = DateTime.UtcNow
            };

            var jsonMessage = JsonConvert.SerializeObject(activityMessage);
            jsonMessage = jsonMessage.Replace("\"", "'");

            var mqttMessage = new MQTTMessage()
            {
                FromInstanceId = Program.InstanceConfig.ServerInstanceID,
                ToInstancesIds = toIds,
                Type = "CLIENT_CONFIGURATION_ACTIVITY",
                Message = jsonMessage
            };

            _mqttService.SendMQTTMessageToMultipleInstances(tenantId, toIds, mqttMessage);
        }
    }
}


using Newtonsoft.Json;
using NServiceDiscovery.Entity;
using NServiceDiscovery.MQTT;
using NServiceDiscovery.Repository;
using System.Collections.Generic;
using System.Linq;

namespace NServiceDiscoveryAPI.Services
{
    public class PublishChangesService : IPublishChangesService
    {
        private IMQTTService _mqttService;
        private IMemoryDiscoveryPeerRepository _discoveryPeerRepository;

        public PublishChangesService(IMQTTService mqttService, IMemoryDiscoveryPeerRepository peersRepository)
        {
            _mqttService = mqttService;
            _discoveryPeerRepository = peersRepository;
        }

        private List<string> GetPeerIds()
        {
            var peers = _discoveryPeerRepository.GetAll();
            var toIds = peers.Select(p => p.ServerInstanceID).Distinct().ToList();

            return toIds;
        }

        public void PublishAddedOrUpdatedInstance(Instance instance, string type = "ADD_INSTANCE")
        {
            var toIds = GetPeerIds();

            if (toIds.Count == 0)
            {
                return;
            }

            var jsonMessage = JsonConvert.SerializeObject(instance);
            jsonMessage = jsonMessage.Replace("\"", "'");

            var mqttMessage = new MQTTMessage()
            {
                FromInstanceId = Program.InstanceConfig.ServerInstanceID,
                ToInstancesIds = toIds,
                Type = type,
                Message = jsonMessage
            };

            _mqttService.SendMQTTMessageToMultipleInstances(instance.TenantId, toIds, mqttMessage);
        }

        public void PublishInstanceStatusChange(string tenantId, string appName, string instanceId, string status, long dirtyTimestamp)
        {
            var toIds = GetPeerIds();

            if (toIds.Count == 0)
            {
                return;
            }

            var changeMessage = new MQTTInstanceChangeStatusMessageContent()
            {
                AppName = appName,
                TenantId = tenantId,
                InstanceId = instanceId,
                Status = status,
                LastDirtyTimestamp = dirtyTimestamp
            };

            var jsonMessage = JsonConvert.SerializeObject(changeMessage);
            jsonMessage = jsonMessage.Replace("\"", "'");

            var mqttMessage = new MQTTMessage()
            {
                FromInstanceId = Program.InstanceConfig.ServerInstanceID,
                ToInstancesIds = toIds,
                Type = "CHANGE_INSTANCE_STATUS",
                Message = jsonMessage
            };

            _mqttService.SendMQTTMessageToMultipleInstances(tenantId, toIds, mqttMessage);
        }

        public void PublishDeletedInstance(string tenantId, string appName, string instanceId)
        {
            var toIds = GetPeerIds();

            if (toIds.Count == 0)
            {
                return;
            }

            var deleteMessage = new MQTTInstanceDeleteMessageContent()
            {
                AppName = appName,
                TenantId = tenantId,
                InstanceId = instanceId
            };

            var jsonMessage = JsonConvert.SerializeObject(deleteMessage);
            jsonMessage = jsonMessage.Replace("\"", "'");

            var mqttMessage = new MQTTMessage()
            {
                FromInstanceId = Program.InstanceConfig.ServerInstanceID,
                ToInstancesIds = toIds,
                Type = "DELETE_INSTANCE",
                Message = jsonMessage
            };

            _mqttService.SendMQTTMessageToMultipleInstances(tenantId, toIds, mqttMessage);
        }
    }
}

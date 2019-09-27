
using Newtonsoft.Json;
using NServiceDiscovery.Entity;
using NServiceDiscovery.MQTT;
using NServiceDiscovery.Repository;
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

        public void PublishAddedorUpdatedInstance(Instance instance)
        {
            var peers = _discoveryPeerRepository.GetAll();
            var toIds = peers.Select(p => p.ServerInstanceID).Distinct().ToList();

            var jsonMessage = JsonConvert.SerializeObject(instance);
            jsonMessage = jsonMessage.Replace("\"", "'");

            var mqttMessage = new MQTTMessage()
            {
                FromInstanceId = Program.InstanceConfig.ServerInstanceID,
                ToInstancesIds = toIds,
                Type = "ADD_UPDATE_INSTANCE",
                Message = jsonMessage
            };

            _mqttService.SendMQTTMessageToMultipleInstances(instance.TenantId, toIds, mqttMessage);
        }

        public void PublishDeletedInstance(string tenantId, string instanceId)
        {
            var peers = _discoveryPeerRepository.GetAll();
            var toIds = peers.Select(p => p.ServerInstanceID).Distinct().ToList();

            var jsonMessage = instanceId;

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

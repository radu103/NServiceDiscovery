using NServiceDiscovery.Entity;

namespace NServiceDiscoveryAPI.Services
{
    public interface IPublishChangesService
    {
        void PublishAddedOrUpdatedInstance(Instance instance, string type);

        void PublishInstanceStatusChange(string tenantId, string appName, string instanceId, string status, long dirtyTimestamp);

        void PublishDeletedInstance(string tenantId, string appName, string instanceId);
    }
}
using NServiceDiscovery.Entity;

namespace NServiceDiscoveryAPI.Services
{
    public interface IPublishChangesService
    {
        void PublishAddedorUpdatedInstance(Instance instance);

        void PublishDeletedInstance(string tenantId, string instanceId);
    }
}
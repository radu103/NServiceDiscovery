using NServiceDiscovery.Entity;

namespace NServiceDiscoveryAPI.Services
{
    public interface IInstanceStatusService
    {
        InstanceStatus GetStatus();
    }
}
using NServiceDiscovery.Common.ServiceBase;

namespace NServiceDiscoveryAPI.Services
{
    public interface IInstanceHealthService
    {
        ServiceHealth GetHealth();
    }
}
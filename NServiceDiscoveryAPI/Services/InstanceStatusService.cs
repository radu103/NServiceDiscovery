
using NServiceDiscovery.Entity;

namespace NServiceDiscoveryAPI.Services
{
    public class InstanceStatusService : IInstanceStatusService
    {
        public InstanceStatus GetStatus()
        {
            InstanceStatus status = new InstanceStatus()
            {
                ServerInstanceID = Program.InstanceConfig.ServerInstanceID,
                HttpEndpoint = string.Empty,
                SecureHttpEndpoint = string.Empty
            };

            return status;
        }
    }
}

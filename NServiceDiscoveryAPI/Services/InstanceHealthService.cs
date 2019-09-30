using NServiceDiscovery.Common.ServiceBase;
using NServiceDiscovery.RuntimeInMemory;
using System;
using System.Linq;

namespace NServiceDiscoveryAPI.Services
{
    public class InstanceHealthService : IInstanceHealthService
    {
        public static ServiceHealth Health = new ServiceHealth();

        public ServiceHealth GetHealth()
        {
            InstanceHealthService.Health.PeerCount = Memory.Peers.Count;
            
            InstanceHealthService.Health.DiscoveryActiveClientCount = Memory.DiscoveryClients.Count(c => c.LastUpdateTimestamp.AddSeconds(Program.InstanceConfig.ClientDeactivateInSecs) > DateTime.UtcNow);
            InstanceHealthService.Health.ConfigurationActiveClientCount = Memory.ConfigurationClients.Count(c => c.LastUpdateTimestamp.AddSeconds(Program.InstanceConfig.ClientDeactivateInSecs) > DateTime.UtcNow);

            InstanceHealthService.Health.ActiveClientCount = InstanceHealthService.Health.DiscoveryActiveClientCount + InstanceHealthService.Health.ConfigurationActiveClientCount;
            InstanceHealthService.Health.TotalClientCount = Memory.DiscoveryClients.Count + Memory.ConfigurationClients.Count;

            InstanceHealthService.Health.AppsCount = ServicesRuntime.AllApplications.Applications.Count;
            InstanceHealthService.Health.AppsVersion = ServicesRuntime.AllApplications.VersionsDelta;

            InstanceHealthService.Health.TenantCount = Program.Tenants.Count;

            InstanceHealthService.Health.GeneralConfigurationKeysCount = Memory.ConfigurationStore.AllKeyValues.Count;
            InstanceHealthService.Health.GeneralConfigurationVersion = Memory.ConfigurationStore.VersionsDelta;

            return InstanceHealthService.Health;
        }
    }
}

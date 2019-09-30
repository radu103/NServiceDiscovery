using System.Collections.Generic;
using NServiceDiscovery.Entity;

namespace NServiceDiscovery.Repository
{
    public interface IMemoryGeneralConfigurationClientRepository
    {
        void Add(DiscoveryClient newClient);
        List<DiscoveryClient> GetAll();
    }
}
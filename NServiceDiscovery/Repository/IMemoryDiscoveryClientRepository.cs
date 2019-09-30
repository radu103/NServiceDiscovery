using System.Collections.Generic;
using NServiceDiscovery.Entity;

namespace NServiceDiscovery.Repository
{
    public interface IMemoryDiscoveryClientRepository
    {
        void Add(DiscoveryClient newClient);
        List<DiscoveryClient> GetAll();
    }
}
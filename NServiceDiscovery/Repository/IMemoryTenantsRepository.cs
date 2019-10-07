using System.Collections.Generic;
using NServiceDiscovery.Entity;

namespace NServiceDiscovery.Repository
{
    public interface IMemoryTenantsRepository
    {
        List<Tenant> GetAll();

        void Add(Tenant tenant);
    }
}
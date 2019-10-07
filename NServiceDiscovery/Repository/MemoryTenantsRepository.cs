using NServiceDiscovery.Entity;
using System.Collections.Generic;

namespace NServiceDiscovery.Repository
{
    public class MemoryTenantsRepository : IMemoryTenantsRepository
    {
        private List<Tenant> Tenants = new List<Tenant>();

        public List<Tenant> GetAll()
        {
            return Tenants;
        }

        public void Add(Tenant tenant)
        {
            Tenants.Add(tenant);
        }
    }
}

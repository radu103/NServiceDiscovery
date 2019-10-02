using NServiceDiscovery.Entity;
using System.Collections.Generic;

namespace NServiceDiscovery.Repository
{
    public class MemoryTenantsRepository : IMemoryTenantsRepository
    {
        public List<Tenant> GetAll()
        {
            var list = new List<Tenant>();

            list.Add(new Tenant()
            {
                TenantId = "public-dev"
            });

            list.Add(new Tenant()
            {
                TenantId = "public-qa"
            });

            list.Add(new Tenant()
            {
                TenantId = "public-production"
            });

            return list;
        }
    }
}

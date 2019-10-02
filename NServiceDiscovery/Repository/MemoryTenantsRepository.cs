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
                TenantId = "public"
            });

            list.Add(new Tenant()
            {
                TenantId = "public"
            });

            list.Add(new Tenant()
            {
                TenantId = "public"
            });

            return list;
        }
    }
}

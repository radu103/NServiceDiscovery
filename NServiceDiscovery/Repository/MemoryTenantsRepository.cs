﻿using NServiceDiscovery.Configuration;
using NServiceDiscovery.Entity;
using System.Collections.Generic;

namespace NServiceDiscovery.Repository
{
    public class MemoryTenantsRepository
    {
        private string repoTenantId = DefaultConfigurationData.DefaultTenantID;

        public MemoryTenantsRepository(string tenantId)
        {
            if (!string.IsNullOrEmpty(tenantId))
            {
                repoTenantId = tenantId;
            }
        }

        public List<Tenant> GetAll()
        {
            var list = new List<Tenant>();

            list.Add(new Tenant()
            {
                TenantId = "public",
                TenantType = "dev"
            });

            list.Add(new Tenant()
            {
                TenantId = "public2",
                TenantType = "dev"
            });

            return list;
        }
    }
}
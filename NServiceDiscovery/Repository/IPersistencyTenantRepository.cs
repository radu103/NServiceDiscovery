using System.Collections.Generic;
using NServiceDiscovery.Persistency;

namespace NServiceDiscovery.Repository
{
    public interface IPersistencyTenantRepository
    {
        List<PersistencyTenant> LoadPersistedTenants();
    }
}
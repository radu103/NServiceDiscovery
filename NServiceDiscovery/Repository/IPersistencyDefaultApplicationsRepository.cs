using System.Collections.Generic;
using NServiceDiscovery.Persistency;

namespace NServiceDiscovery.Repository
{
    public interface IPersistencyDefaultApplicationsRepository
    {
        List<PersistencyDefaultApp> LoadPersistedDefaultApplications();
    }
}
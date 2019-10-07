using NServiceDiscovery.Persistency;

namespace NServiceDiscovery.Repository
{
    public interface IPersistencyApplicationsRepository
    {
        PersistencyApplications LoadPersistedApplications();
    }
}
using NServiceDiscovery.Persistency;

namespace NServiceDiscovery.Repository
{
    public interface IPersistencyAppConfigurationsRepository
    {
        PersistencyAppConfigurations LoadPersistedApplicationConfigurations();
    }
}
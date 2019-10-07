using NServiceDiscovery.Persistency;

namespace NServiceDiscovery.Repository
{
    public interface IPersistencyGeneralConfigurationsRepository
    {
        PersistencyAppConfigurations LoadPersistedGeneralConfigurations();
    }
}
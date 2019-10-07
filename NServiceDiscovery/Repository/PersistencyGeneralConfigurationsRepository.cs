using MongoRepository;
using NServiceDiscovery.Persistency;

namespace NServiceDiscovery.Repository
{
    public class PersistencyGeneralConfigurationsRepository : IPersistencyGeneralConfigurationsRepository
    {
        private MongoRepository<PersistencyGeneralConfigurations> _repo;

        public PersistencyGeneralConfigurationsRepository(IMongoDBSettings settings)
        {
            _repo = new MongoRepository<PersistencyGeneralConfigurations>(settings.MongoDbUrl);
        }

        public PersistencyGeneralConfigurationsRepository(MongoRepository<PersistencyGeneralConfigurations> repo)
        {
            _repo = repo;
        }

        public PersistencyAppConfigurations LoadPersistedGeneralConfigurations()
        {
            var persistedApps = new PersistencyAppConfigurations();

            return persistedApps;
        }
    }
}
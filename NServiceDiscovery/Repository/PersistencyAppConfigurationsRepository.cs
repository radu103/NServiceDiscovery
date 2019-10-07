using MongoRepository;
using NServiceDiscovery.Persistency;

namespace NServiceDiscovery.Repository
{
    public class PersistencyAppConfigurationsRepository : IPersistencyAppConfigurationsRepository
    {
        private MongoRepository<PersistencyAppConfigurations> _repo;

        public PersistencyAppConfigurationsRepository(IMongoDBSettings settings)
        {
            _repo = new MongoRepository<PersistencyAppConfigurations>(settings.MongoDbUrl);
        }

        public PersistencyAppConfigurationsRepository(MongoRepository<PersistencyAppConfigurations> repo)
        {
            _repo = repo;
        }

        public PersistencyAppConfigurations LoadPersistedApplicationConfigurations()
        {
            var persistedApps = new PersistencyAppConfigurations();

            return persistedApps;
        }
    }
}
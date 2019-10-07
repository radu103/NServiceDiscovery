using MongoRepository;
using NServiceDiscovery.Persistency;

namespace NServiceDiscovery.Repository
{
    public class PersistencyApplicationsRepository : IPersistencyApplicationsRepository
    {
        private MongoRepository<PersistencyApplications> _repo;

        public PersistencyApplicationsRepository(IMongoDBSettings settings)
        {
            _repo = new MongoRepository<PersistencyApplications>(settings.MongoDbUrl);
        }

        public PersistencyApplicationsRepository(MongoRepository<PersistencyApplications> repo)
        {
            _repo = repo;
        }

        public PersistencyApplications LoadPersistedApplications()
        {
            var persistedApps = new PersistencyApplications();

            return persistedApps;
        }
    }
}
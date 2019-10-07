using MongoRepository;
using NServiceDiscovery.Persistency;
using System.Collections.Generic;

namespace NServiceDiscovery.Repository
{
    public class PersistencyDefaultApplicationsRepository : IPersistencyDefaultApplicationsRepository
    {
        private MongoRepository<PersistencyDefaultApp> _repo;

        public PersistencyDefaultApplicationsRepository()
        {
            _repo = new MongoRepository<PersistencyDefaultApp>();
        }

        public PersistencyDefaultApplicationsRepository(MongoRepository<PersistencyDefaultApp> repo)
        {
            _repo = repo;
        }

        public List<PersistencyDefaultApp> LoadPersistedDefaultApplications()
        {
            var defaultApps = new List<PersistencyDefaultApp>();

            return defaultApps;
        }
    }
}
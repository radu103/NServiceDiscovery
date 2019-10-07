using MongoRepository;
using NServiceDiscovery.Persistency;
using System.Collections.Generic;

namespace NServiceDiscovery.Repository
{
    public class PersistencyTenantRepository : IPersistencyTenantRepository
    {
        private MongoRepository<PersistencyTenant> _repo;

        public PersistencyTenantRepository(IMongoDBSettings settings)
        {
            _repo = new MongoRepository<PersistencyTenant>(settings.MongoDbUrl);
        }

        public PersistencyTenantRepository(MongoRepository<PersistencyTenant> repo)
        {
            _repo = repo;
        }

        public List<PersistencyTenant> LoadPersistedTenants()
        {
            var persistedTenants = new List<PersistencyTenant>();

            return persistedTenants;
        }
    }
}
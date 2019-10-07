using MongoRepository;
using NServiceDiscovery.Persistency;
using System.Collections.Generic;

namespace NServiceDiscovery.Repository
{
    public class PersistencyTenantRepository : IPersistencyTenantRepository
    {
        private MongoRepository<PersistencyTenant> _repo;

        public PersistencyTenantRepository()
        {
            _repo = new MongoRepository<PersistencyTenant>();
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
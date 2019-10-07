using MongoRepository;
using NServiceDiscovery.Persistency;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var repoTenants = _repo.Where(t => t.ExpireDate >= DateTime.UtcNow).ToList();
            
            if(repoTenants.Count == 0)
            {
                _repo.Add(new PersistencyTenant
                {
                    TenantId = "public-dev",
                    TenantToken = "",
                    ExpireDate = DateTime.UtcNow.AddYears(3)
                });

                _repo.Add(new PersistencyTenant
                {
                    TenantId = "public-qa",
                    TenantToken = "",
                    ExpireDate = DateTime.UtcNow.AddYears(3)
                });

                _repo.Add(new PersistencyTenant
                {
                    TenantId = "public-production",
                    TenantToken = "",
                    ExpireDate = DateTime.UtcNow.AddYears(3)
                });

                repoTenants = _repo.Where(t => t.ExpireDate >= DateTime.UtcNow).ToList();
            }

           return repoTenants;
        }
    }
}
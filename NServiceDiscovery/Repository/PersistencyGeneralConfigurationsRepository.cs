﻿using MongoRepository;
using NServiceDiscovery.Persistency;

namespace NServiceDiscovery.Repository
{
    public class PersistencyGeneralConfigurationsRepository : IPersistencyGeneralConfigurationsRepository
    {
        private MongoRepository<PersistencyGeneralConfigurations> _repo;

        public PersistencyGeneralConfigurationsRepository()
        {
            _repo = new MongoRepository<PersistencyGeneralConfigurations>();
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
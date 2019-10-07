using System;

namespace NServiceDiscovery.Persistency
{
    public class PersistencyTenant : MongoRepository.Entity
    {
        public string TenantId { get; set; }

        public string TenantToken { get; set; }

        public DateTime ExpireDate { get; set; }
    }
}

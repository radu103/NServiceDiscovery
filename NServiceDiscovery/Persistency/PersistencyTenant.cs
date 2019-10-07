namespace NServiceDiscovery.Persistency
{
    public class PersistencyTenant : MongoRepository.Entity
    {
        public string TenantId { get; set; }

        public string TenantToken { get; set; }
    }
}

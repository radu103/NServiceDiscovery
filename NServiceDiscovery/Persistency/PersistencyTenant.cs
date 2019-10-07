using MongoDB.Bson.Serialization.Attributes;
using System;

namespace NServiceDiscovery.Persistency
{
    public class PersistencyTenant : MongoRepository.Entity
    {
        [BsonElement("tenantId")]
        public string TenantId { get; set; }

        [BsonElement("tenantToken")]
        public string TenantToken { get; set; }

        [BsonElement("expireDate")]
        public DateTime ExpireDate { get; set; }
    }
}

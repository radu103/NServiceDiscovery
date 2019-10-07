using MongoDB.Bson.Serialization.Attributes;
using NServiceDiscovery.Entity;
using System;
using System.Collections.Generic;

namespace NServiceDiscovery.Persistency
{
    public class PersistencyApplications : MongoRepository.Entity
    {
        [BsonElement("tenantId")]
        public string TenantId { get; set; }

        [BsonElement("instanceId")]
        public string InstanceId { get; set; }

        [BsonElement("utcTimestamp")]
        public DateTime UTCTimestamp { get; set; }

        [BsonElement("versionsDelta")]
        public long VersionsDelta { get; set; }

        [BsonElement("applications")]
        public List<Application> Applications { get; set; }
    }
}

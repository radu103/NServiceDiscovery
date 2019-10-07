using MongoDB.Bson.Serialization.Attributes;
using NServiceDiscovery.Entity;
using System;
using System.Collections.Generic;

namespace NServiceDiscovery.Persistency
{
    public class PersistencyAppConfigurations : MongoRepository.Entity
    {
        [BsonElement("tenantId")]
        public string TenantId { get; set; }

        [BsonElement("appName")]
        public string AppName { get; set; }

        [BsonElement("instanceId")]
        public string InstanceId { get; set; }

        [BsonElement("utcTimestamp")]
        public DateTime UTCTimestamp { get; set; }

        [BsonElement("versionsDelta")]
        public long VersionsDelta { get; set; }

        [BsonElement("keyValues")]
        public List<StoreKeyValue> KeyValues { get; set; }
    }
}

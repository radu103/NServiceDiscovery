using NServiceDiscovery.Entity;
using System;
using System.Collections.Generic;

namespace NServiceDiscovery.Persistency
{
    public class PersistencyAppConfigurations : MongoRepository.Entity
    {
        public string ID { get; set; }

        public string TenantId { get; set; }

        public string AppName { get; set; }

        public string InstanceId { get; set; }

        public DateTime UTCTimestamp { get; set; }

        public long VersionsDelta { get; set; }

        public List<StoreKeyValue> KeyValues { get; set; }
    }
}

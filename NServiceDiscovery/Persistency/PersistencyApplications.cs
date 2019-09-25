using NServiceDiscovery.Entity;
using System;
using System.Collections.Generic;

namespace NServiceDiscovery.Persistency
{
    public class PersistencyApplications
    {
        public string TenantId { get; set; }

        public string TenantType { get; set; }

        public string InstanceId { get; set; }

        public DateTime UTCTimestamp { get; set; }

        public long VersionsDelta { get; set; }

        public List<Application> Applications { get; set; }
    }
}

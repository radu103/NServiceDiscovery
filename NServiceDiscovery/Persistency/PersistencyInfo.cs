using NServiceDiscovery.Entity;
using System;
using System.Collections.Generic;

namespace NServiceDiscovery.Persistency
{
    public class PersistencyInfo
    {
        public Tenant SynchTenant { get; set; }

        public bool SynchStarted { get; set; }

        public string LastSynchStarterId { get; set; }

        public DateTime SyncStart { get; set; }

        public DateTime SyncEnd { get; set; }

        public AllApplications Apps { get; set; }

        public List<StoreKeyValue> KeyValues { get; set; }

        public List<DiscoveryPeer> Peers { get; set; }

        public List<PeerSyncResponse> PeerSyncResponses { get; set; }
    }
}

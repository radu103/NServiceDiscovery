using System.Collections.Generic;
using NServiceDiscovery.Entity;

namespace NServiceDiscoveryAPI.Services
{
    public interface IPersistencyService
    {
        int GetPersistencyTimerInterval();
        void SetPersistencyTimerInterval(int seconds);
        bool StartSyncProcess(Tenant tenant, List<DiscoveryPeer> peers, AllApplications apps, List<StoreKeyValue> generalKeyValues, int seconds);
        bool StopSyncProcess(Tenant tenant);
    }
}
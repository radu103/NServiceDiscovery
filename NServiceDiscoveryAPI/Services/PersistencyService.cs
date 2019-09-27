using Newtonsoft.Json;
using NServiceDiscovery.Entity;
using NServiceDiscovery.Persistency;
using System;
using System.Collections.Generic;
using System.Timers;

namespace NServiceDiscoveryAPI.Services
{
    public class PersistencyService : IPersistencyService
    {
        private static Timer _PersistencyTimer = new Timer();
        private int PersistencyTimerInterval = 300;

        public static PersistencyInfo SyncInfo = new PersistencyInfo();

        public PersistencyService(){}

        private static void OnStartPersistencySync(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("START PERSYSTENCY SYNC : " + DateTime.Now.ToString());
        }

        public int GetPersistencyTimerInterval()
        {
            return PersistencyTimerInterval;
        }

        public void SetPersistencyTimerInterval(int seconds)
        {
            PersistencyTimerInterval = seconds;

            _PersistencyTimer.Stop();
            _PersistencyTimer.Dispose();

            _PersistencyTimer = new Timer(PersistencyTimerInterval * 1000);
            _PersistencyTimer.AutoReset = true;
            _PersistencyTimer.Enabled = true;
            _PersistencyTimer.Elapsed += OnStartPersistencySync;
        }

        public bool StartSyncProcess(string requesterId, List<DiscoveryPeer> peers, AllApplications apps, List<StoreKeyValue> generalKeyValues, int seconds)
        {
            PersistencyService.SyncInfo.SynchStarted = true;

            PersistencyService.SyncInfo.LastSynchStarterId = requesterId;

            PersistencyService.SyncInfo.SyncStart = DateTime.Now;
            PersistencyService.SyncInfo.SyncEnd = DateTime.Now.AddSeconds(seconds);

            PersistencyService.SyncInfo.Apps = JsonConvert.DeserializeObject<AllApplications>(JsonConvert.SerializeObject(apps));

            PersistencyService.SyncInfo.KeyValues = JsonConvert.DeserializeObject<List<StoreKeyValue>>(JsonConvert.SerializeObject(generalKeyValues));

            PersistencyService.SyncInfo.Peers = JsonConvert.DeserializeObject<List<DiscoveryPeer>>(JsonConvert.SerializeObject(peers));

            PersistencyService.SyncInfo.PeerSyncResponses = new List<PeerSyncResponse>();

            return true;
        }

        public bool StopSyncProcess()
        {
            PersistencyService.SyncInfo.SynchStarted = false;

            PersistencyService.SyncInfo.LastSynchStarterId = string.Empty;
            PersistencyService.SyncInfo.SyncEnd = DateTime.Now;

            PersistencyService.SyncInfo.Peers = new List<DiscoveryPeer>();
            PersistencyService.SyncInfo.PeerSyncResponses = new List<PeerSyncResponse>();

            return true;
        }
    }
}

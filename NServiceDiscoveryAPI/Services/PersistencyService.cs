using Newtonsoft.Json;
using NServiceDiscovery.Entity;
using NServiceDiscovery.Persistency;
using NServiceDiscovery.Repository;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using NServiceDiscovery.MQTT;

namespace NServiceDiscoveryAPI.Services
{
    public class PersistencyService : IPersistencyService
    {
        private Timer _PersistencyTimer = new Timer();
        private int PersistencyTimerInterval = 300;

        public static List<PersistencyInfo> SyncInfos = new List<PersistencyInfo>();

        public PersistencyService(){}

        private void OnStartPersistencySync(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("START PERSYSTENCY SYNC : " + DateTime.Now.ToString());

            var peerRepo = new MemoryDiscoveryPeerRepository();
            var peers = peerRepo.GetAll();

            foreach (var tenant in Program.Tenants)
            {
                var memoryRepo = new MemoryServicesRepository(tenant.TenantId + "-" + tenant.TenantType);
                var tenantSynchInfo = memoryRepo.GetTenantSyncInfo();

                if(tenantSynchInfo != null)
                {
                    var keyValueRepo = new MemoryConfigurationStoreRepository(tenant.TenantId + "-" + tenant.TenantType);
                    var keysSyncInfo = keyValueRepo.GetAllKeysSyncInfo();

                    if(keysSyncInfo != null)
                    {
                        StartSyncProcess(tenant, peers, tenantSynchInfo, keysSyncInfo, Program.InstanceConfig.PersistencySyncWaitSeconds);
                    }
                }
            }
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

        public bool StartSyncProcess(Tenant tenant, List<DiscoveryPeer> peers, AppsSyncInfo apps, KeysSyncInfo keys, int seconds)
        {
            PersistencyInfo synchInfo = null;

            // save sync state infos for tenant

            lock(SyncInfos){

                synchInfo = SyncInfos.SingleOrDefault(s => s.SynchTenant.TenantId.CompareTo(tenant.TenantId) == 0 && s.SynchTenant.TenantType.CompareTo(tenant.TenantType) == 0);

                if (synchInfo == null)
                {
                    synchInfo = new PersistencyInfo();
                    synchInfo.SynchTenant = tenant;
                    SyncInfos.Add(synchInfo);
                }
            }

            synchInfo.SynchStarted = true;
            synchInfo.LastSynchStarterId = Program.InstanceConfig.ServerInstanceID;
            synchInfo.SyncStart = DateTime.Now;
            synchInfo.SyncEnd = DateTime.Now.AddSeconds(seconds);
            synchInfo.Apps = JsonConvert.DeserializeObject<AllApplications>(JsonConvert.SerializeObject(apps.Apps));
            synchInfo.KeyValues = JsonConvert.DeserializeObject<List<StoreKeyValue>>(JsonConvert.SerializeObject(keys.Keys));
            synchInfo.Peers = JsonConvert.DeserializeObject<List<DiscoveryPeer>>(JsonConvert.SerializeObject(peers));
            synchInfo.PeerSyncResponses = new List<PeerSyncResponse>();
            synchInfo.AppsMD5 = apps.MD5Hash;
            synchInfo.KeysMD5 = keys.MD5Hash;

            // send MQTT PERSISTENCY_SYNC message
            if (!string.IsNullOrEmpty(synchInfo.AppsMD5))
            {
                var syncMessage = new MQTTPersistencySyncAppsMessageContent()
                {
                    TenantId = tenant.TenantId + "-" + tenant.TenantType,
                    PeerId = Program.InstanceConfig.ServerInstanceID,
                    AppsMd5Hash = synchInfo.AppsMD5
                };

                var toIds = new List<string>();
                toIds.Add("ALL");

                var jsonMessage = JsonConvert.SerializeObject(syncMessage);
                jsonMessage = jsonMessage.Replace("\"", "'");

                var mqttMessage = new MQTTMessage()
                {
                    FromInstanceId = Program.InstanceConfig.ServerInstanceID,
                    ToInstancesIds = toIds,
                    Type = "PERSISTENCY_SYNC_APPS",
                    Message = jsonMessage
                };

                Program.mqttService.SendMQTTMessageToAll(synchInfo.SynchTenant.TenantId + "-" + synchInfo.SynchTenant.TenantType, mqttMessage);
            }

            if (!string.IsNullOrEmpty(synchInfo.KeysMD5))
            {
                var syncMessage = new MQTTPersistencySyncKeysMessageContent()
                {
                    TenantId = tenant.TenantId + "-" + tenant.TenantType,
                    PeerId = Program.InstanceConfig.ServerInstanceID,
                    KeysMd5Hash = synchInfo.KeysMD5
                };

                var toIds = new List<string>();
                toIds.Add("ALL");

                var jsonMessage = JsonConvert.SerializeObject(syncMessage);
                jsonMessage = jsonMessage.Replace("\"", "'");

                var mqttMessage = new MQTTMessage()
                {
                    FromInstanceId = Program.InstanceConfig.ServerInstanceID,
                    ToInstancesIds = toIds,
                    Type = "PERSISTENCY_SYNC_KEYS",
                    Message = jsonMessage
                };

                Program.mqttService.SendMQTTMessageToAll(synchInfo.SynchTenant.TenantId + "-" + synchInfo.SynchTenant.TenantType, mqttMessage);
            }

            return true;
        }

        public bool StopSyncProcess(Tenant tenant)
        {
            PersistencyInfo synchInfo = null;

            lock (SyncInfos)
            {
                synchInfo = SyncInfos.SingleOrDefault(s => s.SynchTenant.TenantId.CompareTo(tenant.TenantId) == 0 && s.SynchTenant.TenantType.CompareTo(tenant.TenantType) == 0);
            }
        
            if(synchInfo != null)
            {
                synchInfo.SynchStarted = false;

                synchInfo.LastSynchStarterId = string.Empty;
                synchInfo.SyncEnd = DateTime.Now;
                
                synchInfo.Peers = new List<DiscoveryPeer>();
                synchInfo.PeerSyncResponses = new List<PeerSyncResponse>();

                return true;
            }

            return false;
        }
    }
}

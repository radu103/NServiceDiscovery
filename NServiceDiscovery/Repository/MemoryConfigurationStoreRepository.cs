using Newtonsoft.Json;
using NServiceDiscovery.Configuration;
using NServiceDiscovery.Entity;
using NServiceDiscovery.RuntimeInMemory;
using NServiceDiscovery.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NServiceDiscovery.Repository
{
    public class MemoryConfigurationStoreRepository
    {
        private string repoTenantId = DefaultConfigurationData.DefaultTenantID + "-" + DefaultConfigurationData.DefaultTenantType;

        private void IncreaseVersionForGeneralStore()
        {
            Memory.ConfigurationStore.VersionsDelta += 1;
        }

        private void IncreaseVersionForServicesRuntime()
        {
            ServicesRuntime.AllApplications.VersionsDelta  += 1;
        }

        public MemoryConfigurationStoreRepository(string tenantId)
        {
            if (!string.IsNullOrEmpty(tenantId))
            {
                repoTenantId = tenantId;
            }
        }

        private Tenant GetTenant()
        {
            var tenant = new Tenant();

            var aux = repoTenantId.Split(new char[] { '-' });

            tenant.TenantId = aux[0];
            tenant.TenantType = aux[1];

            return tenant;
        }

        public bool Add(StoreKeyValue keyValue)
        {
            var existingKey = Memory.ConfigurationStore.AllKeyValues.SingleOrDefault(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.AppName.CompareTo(string.Empty) == 0 && g.Key.CompareTo(keyValue.Key) == 0);

            if (existingKey != null)
            {
                Update(keyValue);
            }
            else
            {
                keyValue.TenantId = repoTenantId;
                Memory.ConfigurationStore.AllKeyValues.Add(keyValue);
            }

            IncreaseVersionForGeneralStore();

            return true;
        }

        public bool AddKeys(List<StoreKeyValue> keyValues)
        {
            var ok = true;

            foreach(StoreKeyValue keyValue in keyValues)
            {
                var kOk = Add(keyValue);
                if (!kOk)
                {
                    ok = false;
                    break;
                }
            }

            IncreaseVersionForGeneralStore();

            return ok;
        }

        public bool AddForApplication(string appName, StoreKeyValue keyValue)
        {
            var existingApp = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if(existingApp == null)
            {
                return false;
            }

            var existingKey = Memory.ConfigurationStore.AllKeyValues.SingleOrDefault(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.AppName.CompareTo(appName) == 0 && g.Key.CompareTo(keyValue.Key) == 0);

            if (existingKey != null)
            {
                Update(keyValue);
            }
            else
            {
                keyValue.TenantId = repoTenantId;
                keyValue.AppName = appName;
                Memory.ConfigurationStore.AllKeyValues.Add(keyValue);

                existingApp.Configuration.Add(keyValue);
            }

            IncreaseVersionForServicesRuntime();

            return true;
        }

        public bool AddKeysForApplication(string appName, List<StoreKeyValue> keyValues)
        {
            var ok = true;

            foreach (StoreKeyValue keyValue in keyValues)
            {
                var kOk = AddForApplication(appName, keyValue);
                if (!kOk)
                {
                    ok = false;
                    break;
                }
            }

            IncreaseVersionForServicesRuntime();

            return ok;
        }

        public bool Update(StoreKeyValue keyValue)
        {
            var existingKey = Memory.ConfigurationStore.AllKeyValues.SingleOrDefault(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.AppName.CompareTo(string.Empty) == 0 && g.Key.CompareTo(keyValue.Key) == 0);

            if (existingKey != null)
            {
                var idx = Memory.ConfigurationStore.AllKeyValues.IndexOf(existingKey);
                Memory.ConfigurationStore.AllKeyValues[idx].Value = keyValue.Value;
            }

            IncreaseVersionForGeneralStore();

            return true;
        }

        public bool UpdateForApplication(string appName, StoreKeyValue keyValue)
        {
            var existingApp = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if (existingApp == null)
            {
                return false;
            }

            var existingKey = Memory.ConfigurationStore.AllKeyValues.SingleOrDefault(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.AppName.CompareTo(appName) == 0 && g.Key.CompareTo(keyValue.Key) == 0);

            if (existingKey != null)
            {
                var idx = Memory.ConfigurationStore.AllKeyValues.IndexOf(existingKey);
                Memory.ConfigurationStore.AllKeyValues[idx].Value = keyValue.Value;

                var existingConf = existingApp.Configuration.SingleOrDefault(c => c.Key.CompareTo(keyValue.Key) == 0);
                var idx2 = existingApp.Configuration.IndexOf(existingConf);
                existingApp.Configuration[idx2].Value = keyValue.Value;
            }

            IncreaseVersionForServicesRuntime();

            return true;
        }

        public bool Delete(string key)
        {
            var existingKey = Memory.ConfigurationStore.AllKeyValues.SingleOrDefault(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.AppName.CompareTo(string.Empty) == 0 && g.Key.CompareTo(key) == 0);

            if (existingKey != null)
            {
                var idx = Memory.ConfigurationStore.AllKeyValues.Remove(existingKey);
            }
            else
            {
                return false;
            }

            IncreaseVersionForGeneralStore();

            return true;
        }

        public bool DeleteForApplication(string appName, string key)
        {
            var existingApp = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if (existingApp == null)
            {
                return false;
            }

            var existingKey = Memory.ConfigurationStore.AllKeyValues.SingleOrDefault(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.AppName.CompareTo(appName) == 0 && g.Key.CompareTo(key) == 0);

            if (existingKey != null)
            {
                Memory.ConfigurationStore.AllKeyValues.Remove(existingKey);

                var existingConf = existingApp.Configuration.SingleOrDefault(c => c.Key.CompareTo(key) == 0);

                if(existingConf != null)
                {
                    existingApp.Configuration.Remove(existingConf);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            IncreaseVersionForServicesRuntime();

            return true;
        }

        public int DeleteKeysForApplication(string appName, List<string> keys)
        {
            var existingApp = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if (existingApp == null)
            {
                return 0;
            }

            var removed = Memory.ConfigurationStore.AllKeyValues.RemoveAll(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.AppName.CompareTo(appName) == 0 && keys.IndexOf(g.Key) >= 0);
            
            if(removed > 0)
            {
                IncreaseVersionForServicesRuntime();
            }

            return removed;
        }

        public StoreKeyValue Get(string key)
        {
            var existingKey = Memory.ConfigurationStore.AllKeyValues.SingleOrDefault(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.AppName.CompareTo(string.Empty) == 0 && g.Key.CompareTo(key) == 0);
            return existingKey;
        }

        public StoreKeyValue GetForApplication(string appName, string key)
        {
            var existingKey = Memory.ConfigurationStore.AllKeyValues.SingleOrDefault(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.AppName.CompareTo(appName) == 0 && g.Key.CompareTo(key) == 0);
            return existingKey;
        }

        public List<StoreKeyValue> GetAll()
        {
            var existingKeys = Memory.ConfigurationStore.AllKeyValues.FindAll(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.AppName.CompareTo(string.Empty) == 0).ToList();
            return existingKeys;
        }

        public List<StoreKeyValue> GetAllForApplication(string appName)
        {
            var existingKeys = Memory.ConfigurationStore.AllKeyValues.FindAll(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.AppName.CompareTo(appName) == 0).ToList();
            return existingKeys;
        }

        public KeysSyncInfo GetAllKeysSyncInfo()
        {
            var info = new KeysSyncInfo();

            Tenant tenant = GetTenant();

            info.TenantId = tenant.TenantId;
            info.TenantType = tenant.TenantType;

            try
            {
                var tenantKeys = Memory.ConfigurationStore.AllKeyValues.FindAll(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.AppName.CompareTo(string.Empty) == 0).ToList();

                tenantKeys = tenantKeys.OrderBy(k => k.Key).ToList();

                var keysJson = JsonConvert.SerializeObject(tenantKeys);
                info.Keys = JsonConvert.DeserializeObject<List<StoreKeyValue>>(keysJson);

                StringBuilder signature = new StringBuilder();

                foreach (var k in info.Keys)
                {
                    signature.Append(k.Key + "_" + k.Value + "_");
                }

                var sigStr = signature.ToString();

                if (!string.IsNullOrEmpty(sigStr))
                {
                    info.MD5Hash = sigStr.GetMd5Hash();
                }
                else
                {
                    info.MD5Hash = string.Empty;
                }

                return info;
            }
            catch (Exception err)
            {
                Console.WriteLine("GetAllKeysSyncInfo ERROR - ", err.Message);
            }

            return null;
        }
    }
}

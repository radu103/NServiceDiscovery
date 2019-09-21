using NServiceDiscovery.Configuration;
using NServiceDiscovery.Entity;
using NServiceDiscovery.RuntimeInMemory;
using System.Collections.Generic;
using System.Linq;

namespace NServiceDiscovery.Repository
{
    public class MemoryConfigurationStoreRepository
    {
        private string repoTenantId = DefaultConfigurationData.DefaultTenantID + "-" + DefaultConfigurationData.DefaultTenantType;

        public MemoryConfigurationStoreRepository(string tenantId)
        {
            if (!string.IsNullOrEmpty(tenantId))
            {
                repoTenantId = tenantId;
            }
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

            return true;
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
    }
}

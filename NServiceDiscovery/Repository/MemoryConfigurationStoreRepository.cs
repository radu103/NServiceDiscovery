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

        public bool Add(GeneralKeyValue keyValue)
        {
            var existingKey = Memory.ConfigurationStore.GeneralKeyValues.SingleOrDefault(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.Key.CompareTo(keyValue.Key) == 0);

            if (existingKey != null)
            {
                Update(keyValue);
            }
            else
            {
                keyValue.TenantId = repoTenantId;
                Memory.ConfigurationStore.GeneralKeyValues.Add(keyValue);
            }

            return true;
        }

        public bool Update(GeneralKeyValue keyValue)
        {
            var existingKey = Memory.ConfigurationStore.GeneralKeyValues.SingleOrDefault(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.Key.CompareTo(keyValue.Key) == 0);

            if (existingKey != null)
            {
                var idx = Memory.ConfigurationStore.GeneralKeyValues.IndexOf(existingKey);
                Memory.ConfigurationStore.GeneralKeyValues[idx].Value = keyValue.Value;
            }

            return true;
        }

        public bool Delete(string key)
        {
            var existingKey = Memory.ConfigurationStore.GeneralKeyValues.SingleOrDefault(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.Key.CompareTo(key) == 0);

            if (existingKey != null)
            {
                var idx = Memory.ConfigurationStore.GeneralKeyValues.Remove(existingKey);
            }
            else
            {
                return false;
            }

            return true;
        }

        public GeneralKeyValue Get(string key)
        {
            var existingKey = Memory.ConfigurationStore.GeneralKeyValues.SingleOrDefault(g => g.TenantId.CompareTo(repoTenantId) == 0 && g.Key.CompareTo(key) == 0);
            return existingKey;
        }

        public List<GeneralKeyValue> GetAll()
        {
            var existingKeys = Memory.ConfigurationStore.GeneralKeyValues.FindAll(g => g.TenantId.CompareTo(repoTenantId) == 0).ToList();
            return existingKeys;
        }
    }
}

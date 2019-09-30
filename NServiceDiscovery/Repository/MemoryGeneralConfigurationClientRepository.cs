using NServiceDiscovery.Entity;
using NServiceDiscovery.RuntimeInMemory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NServiceDiscovery.Repository
{
    public class MemoryGeneralConfigurationClientRepository : IMemoryGeneralConfigurationClientRepository
    {
        public void Add(DiscoveryClient newClient)
        {
            lock (Memory.ConfigurationClients)
            {
                var oldClient = Memory.ConfigurationClients.SingleOrDefault(c => c.ClientHostname.CompareTo(newClient.ClientHostname) == 0);

                if (oldClient == null)
                {
                    Memory.ConfigurationClients.Add(newClient);
                    Memory.ConfigurationClients = Memory.ConfigurationClients.OrderByDescending(p => p.LastUpdateTimestamp).ToList();
                }
                else
                {
                    oldClient.LastUpdateTimestamp = DateTime.UtcNow;
                }
            }
        }

        public List<DiscoveryClient> GetAll()
        {
            return Memory.ConfigurationClients;
        }
    }
}

using NServiceDiscovery.Entity;
using NServiceDiscovery.RuntimeInMemory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NServiceDiscovery.Repository
{
    public class MemoryDiscoveryClientRepository : IMemoryDiscoveryClientRepository
    {
        public void Add(DiscoveryClient newClient)
        {
            lock (Memory.DiscoveryClients)
            {
                var oldClient = Memory.DiscoveryClients.SingleOrDefault(c => c.ClientHostname.CompareTo(newClient.ClientHostname) == 0);

                if (oldClient == null)
                {
                    Memory.DiscoveryClients.Add(newClient);
                    Memory.DiscoveryClients = Memory.DiscoveryClients.OrderByDescending(p => p.LastUpdateTimestamp).ToList();
                }
                else
                {
                    oldClient.LastUpdateTimestamp = DateTime.UtcNow;
                }
            }
        }

        public List<DiscoveryClient> GetAll()
        {
            return Memory.DiscoveryClients;
        }
    }
}

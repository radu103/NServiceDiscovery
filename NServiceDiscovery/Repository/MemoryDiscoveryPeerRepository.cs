using NServiceDiscovery.Entity;
using NServiceDiscovery.RuntimeInMemory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NServiceDiscovery.Repository
{
    public class MemoryDiscoveryPeerRepository : IMemoryDiscoveryPeerRepository
    {
        public void Add(DiscoveryPeer newPeer)
        {
            lock (Memory.Peers)
            {
                Memory.Peers.Add(newPeer);
                Memory.Peers = Memory.Peers.OrderByDescending(p => p.LastUpdateTimestamp).ToList();
            }
        }

        public int EvictPeers(int peerEvictionInSecs)
        {
            var peersEvicted = 0;

            lock (Memory.Peers)
            {
                peersEvicted = Memory.Peers.RemoveAll(p => p.LastUpdateTimestamp.AddSeconds(peerEvictionInSecs) < DateTime.UtcNow);

                if (Memory.Peers.Count > 0)
                {
                    Memory.Peers = Memory.Peers.OrderByDescending(p => p.LastUpdateTimestamp).ToList();
                }
            }

            return peersEvicted;
        }

        public List<DiscoveryPeer> GetAll()
        {
            return Memory.Peers;
        }
    }
}

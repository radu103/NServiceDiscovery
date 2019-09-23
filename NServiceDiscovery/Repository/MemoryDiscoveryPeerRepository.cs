using NServiceDiscovery.Entity;
using NServiceDiscovery.RuntimeInMemory;
using System;
using System.Linq;

namespace NServiceDiscovery.Repository
{
    public class MemoryDiscoveryPeerRepository : IMemoryDiscoveryPeerRepository
    {
        public void Add(DiscoveryPeer newPeer)
        {
            Memory.Peers.Add(newPeer);
            Memory.Peers = Memory.Peers.OrderByDescending(p => p.LastConnectTimestamp).ToList();
        }

        public int EvictPeers(int peerEvictionInSecs)
        {
            var peersEvicted = Memory.Peers.RemoveAll(p => p.LastConnectTimestamp.AddSeconds(peerEvictionInSecs) < DateTime.UtcNow);
            Memory.Peers = Memory.Peers.OrderByDescending(p => p.LastConnectTimestamp).ToList();
            return peersEvicted;
        }
    }
}

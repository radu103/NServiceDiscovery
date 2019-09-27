using NServiceDiscovery.Entity;
using System.Collections.Generic;

namespace NServiceDiscovery.Repository
{
    public interface IMemoryDiscoveryPeerRepository
    {
        void Add(DiscoveryPeer newPeer);

        int EvictPeers(int peerEvictionInSecs);

        List<DiscoveryPeer> GetAll();
    }
}
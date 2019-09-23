using NServiceDiscovery.Entity;

namespace NServiceDiscovery.Repository
{
    public interface IMemoryDiscoveryPeerRepository
    {
        void Add(DiscoveryPeer newPeer);
        int EvictPeers(int peerEvictionInSecs);
    }
}
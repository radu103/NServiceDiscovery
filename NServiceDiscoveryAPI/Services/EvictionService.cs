using NServiceDiscovery.Repository;
using System;
using System.Timers;

namespace NServiceDiscoveryAPI.Services
{
    public class EvictionService : IEvictionService
    {
        private static IMemoryDiscoveryPeerRepository _memoryDiscoveryPeerRepository;

        private Timer _evictionTimer;

        public EvictionService(IMemoryDiscoveryPeerRepository memoryDiscoveryPeerRepository)
        {
            _memoryDiscoveryPeerRepository = memoryDiscoveryPeerRepository;

            _evictionTimer = new Timer(Program.InstanceConfig.EvictionTimerIntervalInSecs * 1000);
            _evictionTimer.AutoReset = true;
            _evictionTimer.Enabled = true;
            _evictionTimer.Elapsed += OnTimedEvent;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Eviction Service cleans at {0:HH:mm:ss.fff}", e.SignalTime);

            // evict outdated peers
            var peersEvicted = _memoryDiscoveryPeerRepository.EvictPeers(Program.InstanceConfig.PeerEvictionInSecs);
            Console.WriteLine("Peers evicted {0}", peersEvicted);

            // evict outdated instances
            // TO DO
        }
    }
}

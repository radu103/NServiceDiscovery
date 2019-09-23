using NServiceDiscovery.RuntimeInMemory;
using System;
using System.Timers;
using System.Linq;

namespace NServiceDiscoveryAPI.Services
{
    public class EvictionService : IEvictionService
    {
        private Timer _evictionTimer;

        public EvictionService()
        {
            _evictionTimer = new Timer(Program.InstanceConfig.EvictionTimerIntervalInSecs * 1000);
            _evictionTimer.AutoReset = true;
            _evictionTimer.Enabled = true;
            _evictionTimer.Elapsed += OnTimedEvent;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Eviction Service cleans at {0:HH:mm:ss.fff}", e.SignalTime);

            // evict outdated peers
            var peersEvicted = Memory.Peers.RemoveAll(p => p.LastConnectTimestamp.AddSeconds(Program.InstanceConfig.PeerEvictionInSecs) < DateTime.UtcNow);
            Console.WriteLine("Peers evicted {0}", peersEvicted);

            // evict outdated instances
            // TO DO
        }
    }
}

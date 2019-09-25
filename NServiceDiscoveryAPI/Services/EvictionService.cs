using NServiceDiscovery.Entity;
using NServiceDiscovery.Repository;
using NServiceDiscovery.RuntimeInMemory;
using NServiceDiscovery.Util;
using System;
using System.Collections.Generic;
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

            // evict outdated instances for all tenants
            lock(ServicesRuntime.AllApplications.Applications)
            {
                foreach (var app in ServicesRuntime.AllApplications.Applications)
                {
                    List<Instance> instancesToRemove = new List<Instance>();
                    foreach (var instance in app.Instances)
                    {
                        var instanceLastUpdate = DateTimeConversions.FromJavaMillis(instance.LastUpdatedTimestamp);
                        if (instanceLastUpdate.AddSeconds(Program.InstanceConfig.EvictionInSecs) < DateTime.UtcNow)
                        {
                            instancesToRemove.Add(instance);
                        }
                    }

                    if(instancesToRemove.Count > 0)
                    {
                        foreach (var inst in instancesToRemove)
                        {
                            app.Instances.Remove(inst);
                        }

                        // increase version after eviction
                        ServicesRuntime.AllApplications.VersionsDelta += 1;
                    }

                    Console.WriteLine("For app '{0}' instances evicted {1}", app.Name, instancesToRemove.Count);
                }
            }
        }
    }
}

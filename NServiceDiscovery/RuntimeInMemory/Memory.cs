using NServiceDiscovery.Entity;
using System.Collections.Generic;

namespace NServiceDiscovery.RuntimeInMemory
{
    public static class Memory
    {
        public static List<DiscoveryPeer> Peers = new List<DiscoveryPeer>();

        public static List<DiscoveryClient> DiscoveryClients = new List<DiscoveryClient>();

        public static List<DiscoveryClient> ConfigurationClients = new List<DiscoveryClient>();

        public static ServicesRuntime Runtime = new ServicesRuntime();

        public static ConfigurationStore ConfigurationStore = new ConfigurationStore();
    }
}

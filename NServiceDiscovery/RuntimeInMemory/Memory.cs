using NServiceDiscovery.Entity;
using System.Collections.Generic;

namespace NServiceDiscovery.RuntimeInMemory
{
    public static class Memory
    {
        public static List<DiscoveryPeer> Peers = new List<DiscoveryPeer>();

        public static ServicesRuntime Runtime = new ServicesRuntime();

        public static ConfigurationStore ConfigurationStore = new ConfigurationStore();
    }
}

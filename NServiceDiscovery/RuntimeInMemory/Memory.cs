using System.Collections.Generic;

namespace NServiceDiscovery.RuntimeInMemory
{
    public static class Memory
    {
        public static List<string> Peers = new List<string>();

        public static ServicesRuntime Runtime = new ServicesRuntime();

        public static ConfigurationStore ConfigurationStore = new ConfigurationStore();
    }
}

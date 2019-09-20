namespace NServiceDiscovery.RuntimeInMemory
{
    public static class Memory
    {
        public static ServicesRuntime Runtime = new ServicesRuntime();

        public static ConfigurationStore ConfigurationStore = new ConfigurationStore();
    }
}

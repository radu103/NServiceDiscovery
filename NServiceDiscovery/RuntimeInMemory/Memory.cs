namespace NServiceDiscovery.RuntimeInMemory
{
    public static class Memory
    {
        public const long TICKS_AT_EPOCH = 621355968000000000; 

        public static ServicesRuntime Runtime = new ServicesRuntime();

        public static ConfigurationStore ConfigurationStore = new ConfigurationStore();
    }
}

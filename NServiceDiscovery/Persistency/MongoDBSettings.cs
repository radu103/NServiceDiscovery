namespace NServiceDiscovery.Persistency
{
    public class MongoDBSettings : IMongoDBSettings
    {
        public string HostName { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string DbName { get; set; }

        public string MongoDbUrl { get; set; }
    }
}

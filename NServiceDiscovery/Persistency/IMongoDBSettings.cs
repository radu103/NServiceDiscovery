namespace NServiceDiscovery.Persistency
{
    public interface IMongoDBSettings
    {
        string DbName { get; set; }
        string HostName { get; set; }
        string MongoDbUrl { get; set; }
        string Password { get; set; }
        int Port { get; set; }
        string User { get; set; }
    }
}
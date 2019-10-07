using MongoDB.Bson.Serialization.Attributes;
using NServiceDiscovery.Entity;
using System.Collections.Generic;

namespace NServiceDiscovery.Persistency
{
    public class PersistencyDefaultApp : MongoRepository.Entity
    {
        [BsonElement("appName")]
        public string AppName { get; set; }

        [BsonElement("appDependencies")]
        public List<string> AppDependencies { get; set; }

        [BsonElement("appConfiguration")]
        public List<StoreKeyValue> AppConfiguration { get; set; }
    }
}

using NServiceDiscovery.Entity;
using System.Collections.Generic;

namespace NServiceDiscovery.Persistency
{
    public class PersistencyDefaultApp : MongoRepository.Entity
    {
        public string AppName { get; set; }

        public List<string> AppDependencies { get; set; }

        public List<StoreKeyValue> AppConfiguration { get; set; }
    }
}

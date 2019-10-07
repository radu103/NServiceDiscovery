using Newtonsoft.Json;
using NServiceDiscovery.Entity;
using System.Collections.Generic;

namespace NServiceDiscovery.Persistency
{
    public class PersistencyDefaultApp : MongoRepository.Entity
    {
        [JsonProperty("appName")]
        public string AppName { get; set; }

        [JsonProperty("appDependencies")]
        public List<string> AppDependencies { get; set; }

        [JsonProperty("appConfiguration")]
        public List<StoreKeyValue> AppConfiguration { get; set; }
    }
}

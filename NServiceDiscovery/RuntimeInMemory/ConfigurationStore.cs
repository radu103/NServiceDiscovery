using Newtonsoft.Json;
using NServiceDiscovery.Entity;
using System.Collections.Generic;

namespace NServiceDiscovery.RuntimeInMemory
{
    public class ConfigurationStore
    {
        [JsonProperty("versions__delta")]
        public long VersionsDelta = 0;

        [JsonProperty("generalKeyValues")]
        public List<StoreKeyValue> AllKeyValues = new List<StoreKeyValue>();
    }
}

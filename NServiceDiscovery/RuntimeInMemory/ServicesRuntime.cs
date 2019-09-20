using Newtonsoft.Json;
using NServiceDiscovery.Entity;

namespace NServiceDiscovery.RuntimeInMemory
{
    public class ServicesRuntime
    {
        [JsonProperty("applications")]
        public static AllApplications AllApplications = new AllApplications();
    }
}

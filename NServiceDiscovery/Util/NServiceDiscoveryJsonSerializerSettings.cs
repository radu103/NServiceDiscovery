
using Newtonsoft.Json;

namespace NServiceDiscovery.Util
{
    public static class NServiceDiscoveryJsonSerializerSettings
    {
        public static JsonSerializerSettings IgnoreMissingPropetiesSettings = new JsonSerializerSettings()
        {
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
    }
}

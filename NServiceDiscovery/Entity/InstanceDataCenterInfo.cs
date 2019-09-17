using Newtonsoft.Json;

namespace NServiceDiscovery.Entity
{
    public class InstanceDataCenterInfo
    {
        //"@class": "com.netflix.appinfo.InstanceInfo$DefaultDataCenterInfo"
        [JsonProperty("@class")]
        public string Class { get; set; } = "com.netflix.appinfo.InstanceInfo$DefaultDataCenterInfo";

        // "name": "MyOwn"
        [JsonProperty("name")]
        public string Name { get; set; } = "MyOwn";
    }
}
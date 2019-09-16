namespace NServiceDiscovery.Entity
{
    public class ServiceInstanceDataCenterInfo
    {
        //"@class": "com.netflix.appinfo.InstanceInfo$DefaultDataCenterInfo"
        public string Class { get; set; } = "com.netflix.appinfo.InstanceInfo$DefaultDataCenterInfo";

        // "name": "MyOwn"
        public string Name { get; set; } = "MyOwn";
    }
}
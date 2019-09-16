using System;

namespace NServiceDiscovery.Entity
{
    public class ServiceInstance
    {
        // equal to hostname ?
        public string InstanceId { get; set; }

        public string AppId { get; set; }

        // "hostName"
        public String HostName { get; set; } = string.Empty;

        // "vipAddress"
        public string VipAddress { get; set; } = string.Empty;

        // "secureVipAddress"
        public string SecureVipAddress { get; set; } = string.Empty;

        // "port"
        public ServicePort Port { get; set; }

        // "securePort"
        public ServicePort SecurePort { get; set; }

        // "countryId"
        public string CountryId { get; set; } = "1";

        // "homePageUrl": "http://APPHOST11:8080"
        public string HomePageUrl { get; set; } = string.Empty;

        // "healthCheckUrl": "http://APPHOST11:8080/healthcheck"
        public string HealthCheckUrl { get; set; } = string.Empty;

        // "isCoordinatingDiscoveryServer": "false"
        public bool isCoordinatingDiscoveryServer { get; set; } = false;

        // "lastUpdatedTimestamp": "1568638944892"
        public DateTime LastUpdatedTimestamp { get; set; }

        // "lastDirtyTimestamp": "1568638937357"
        public DateTime LastDirtyTimestamp { get; set; }

        // "actionType": "MODIFIED"
        public string ActionType { get; set; } = "MODIFIED";

        // "metadata": {}
        public ServiceInstanceMetadata Metadata { get; set; } = new ServiceInstanceMetadata();

        // "leaseInfo" : {}
        public ServiceInstanceLeaseInfo LeaseInfo { get; set; } = new ServiceInstanceLeaseInfo();

        // "dataCenterInfo" : {}
        public ServiceInstanceDataCenterInfo DataCenterInfo { get; set; } = new ServiceInstanceDataCenterInfo();
    }
}

using Newtonsoft.Json;
using System;

namespace NServiceDiscovery.Entity
{
    public class ServiceInstance
    {
        [JsonIgnore]
        [JsonProperty("tenantId")]
        public string TenantId { get; set; } = string.Empty;

        [JsonProperty("appId")]
        public string AppName { get; set; }

        // equal to Hostname
        [JsonProperty("instanceId")]
        public string InstanceId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("hostName")]
        public String HostName { get; set; } = string.Empty;

        [JsonProperty("vipAddress")]
        public string VipAddress { get; set; } = string.Empty;

        [JsonProperty("secureVipAddress")]
        public string SecureVipAddress { get; set; } = string.Empty;

        [JsonProperty("port")]
        public ServicePort Port { get; set; }

        [JsonProperty("securePort")]
        public ServicePort SecurePort { get; set; }

        [JsonProperty("countryId")]
        public string CountryId { get; set; } = "1";

        [JsonProperty("homePageUrl")]
        public string HomePageUrl { get; set; } = string.Empty;

        [JsonProperty("healthCheckUrl")]
        public string HealthCheckUrl { get; set; } = string.Empty;

        [JsonProperty("isCoordinatingDiscoveryServer")]
        public bool isCoordinatingDiscoveryServer { get; set; } = false;

        [JsonProperty("lastUpdatedTimestamp")]
        public DateTime LastUpdatedTimestamp { get; set; }

        [JsonProperty("lastDirtyTimestamp")]
        public DateTime LastDirtyTimestamp { get; set; }

        [JsonProperty("actionType")]
        public string ActionType { get; set; } = "MODIFIED";

        [JsonProperty("metadata")]
        public ServiceInstanceMetadata Metadata { get; set; } = new ServiceInstanceMetadata();

        [JsonProperty("leaseInfo")]
        public ServiceInstanceLeaseInfo LeaseInfo { get; set; } = new ServiceInstanceLeaseInfo();

        [JsonProperty("dataCenterInfo")]
        public ServiceInstanceDataCenterInfo DataCenterInfo { get; set; } = new ServiceInstanceDataCenterInfo();
    }
}

using Newtonsoft.Json;
using System;

namespace NServiceDiscovery.Entity
{
    public class Instance
    {
        [JsonIgnore]
        [JsonProperty("tenantId")]
        public string TenantId { get; set; } = string.Empty;

        [JsonProperty("app")]
        public string AppName { get; set; }

        // equal to Hostname:Port when missing
        [JsonProperty("instanceId")]
        public string InstanceId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("overriddenStatus")]
        public string OverriddenStatus { get; set; } = "UNKNOWN";

        [JsonProperty("statusPageUrl")]
        public string StatusPageUrl { get; set; }

        [JsonProperty("hostName")]
        public String HostName { get; set; } = string.Empty;

        [JsonProperty("ipAddr")]
        public String IpAddress { get; set; } = string.Empty;

        [JsonProperty("vipAddress")]
        public string VipAddress { get; set; } = string.Empty;

        [JsonProperty("secureVipAddress")]
        public string SecureVipAddress { get; set; } = string.Empty;

        [JsonProperty("port")]
        public ApplicationPort Port { get; set; }

        [JsonProperty("securePort")]
        public ApplicationPort SecurePort { get; set; }

        [JsonProperty("countryId")]
        public int CountryId { get; set; } = 1;

        [JsonProperty("homePageUrl")]
        public string HomePageUrl { get; set; } = string.Empty;

        [JsonProperty("healthCheckUrl")]
        public string HealthCheckUrl { get; set; } = string.Empty;

        [JsonProperty("secureHealthCheckUrl")]
        public string SecureHealthCheckUrl { get; set; } = string.Empty;

        [JsonProperty("isCoordinatingDiscoveryServer")]
        public string isCoordinatingDiscoveryServer { get; set; } = "false";

        [JsonProperty("lastUpdatedTimestamp")]
        public string LastUpdatedTimestamp { get; set; }

        [JsonProperty("lastDirtyTimestamp")]
        public string LastDirtyTimestamp { get; set; }

        [JsonProperty("actionType")]
        public string ActionType { get; set; } = "MODIFIED";

        [JsonProperty("metadata")]
        public InstanceMetadata Metadata { get; set; } = new InstanceMetadata();

        [JsonProperty("leaseInfo")]
        public InstanceLeaseInfo LeaseInfo { get; set; } = new InstanceLeaseInfo();

        [JsonProperty("dataCenterInfo")]
        public InstanceDataCenterInfo DataCenterInfo { get; set; } = new InstanceDataCenterInfo();
    }
}

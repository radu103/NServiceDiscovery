using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NServiceDiscovery.Entity
{
    public class Instance
    {
        [JsonIgnore]
        [JsonProperty("tenantId")]
        public string TenantId { get; set; } = string.Empty;

        [JsonProperty("app")]
        public string AppName { get; set; }

        [JsonProperty("appGroupName")]
        public string AppGroupName { get; set; }

        // equal to Hostname:Port when missing
        [JsonProperty("instanceId")]
        public string InstanceId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("overriddenStatus")]
        public string OverriddenStatus { get; set; } = "UNKNOWN";

        [JsonProperty("hostName")]
        public String HostName { get; set; } = string.Empty;

        [JsonProperty("ipAddr")]
        public String IpAddress { get; set; } = string.Empty;

        [JsonProperty("sid")]
        public string Sid { get; set; }

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

        [JsonProperty("statusPageUrl")]
        public string StatusPageUrl { get; set; }

        [JsonProperty("secureHealthCheckUrl")]
        public string SecureHealthCheckUrl { get; set; } = string.Empty;

        [JsonProperty("isCoordinatingDiscoveryServer")]
        public bool isCoordinatingDiscoveryServer { get; set; } = false;

        [JsonProperty("lastUpdatedTimestamp")]
        public long LastUpdatedTimestamp { get; set; }

        [JsonProperty("lastDirtyTimestamp")]
        public long LastDirtyTimestamp { get; set; }

        [JsonProperty("actionType")]
        public string ActionType { get; set; } = "MODIFIED";

        [JsonProperty("asgName", NullValueHandling = NullValueHandling.Ignore)]
        public string AsgName { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; set; }

        [JsonProperty("leaseInfo")]
        public InstanceLeaseInfo LeaseInfo { get; set; } = new InstanceLeaseInfo();

        [JsonProperty("dataCenterInfo")]
        public InstanceDataCenterInfo DataCenterInfo { get; set; } = new InstanceDataCenterInfo();
    }
}

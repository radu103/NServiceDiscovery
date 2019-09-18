using Newtonsoft.Json;
using NServiceDiscovery.Entity;
using System;
using System.Collections.Generic;

namespace NServiceDiscovery.RuntimeInMemory
{
    public class ServicesRuntime
    {
        [JsonIgnore]
        [JsonProperty("tenantId")]
        public string TenantId { get; set; } = string.Empty;

        [JsonProperty("versions__deltam")]
        public string VersionsDelta {
            get { return _VersionsDelta.ToString(); }
            set { _VersionsDelta = Convert.ToInt32(value); }
        }

        [JsonIgnore]
        public int _VersionsDelta { get; set; } = 1;

        [JsonProperty("apps__hashcode")]
        public string AppsHashCode { get; set; } = string.Empty;

        [JsonProperty("application")]
        public static List<Application> Applications = new List<Application>();
    }
}

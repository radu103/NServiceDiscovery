using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NServiceDiscovery.Entity
{
    public class AllApplications
    {
        [JsonIgnore]
        [JsonProperty("tenantId")]
        public string TenantId { get; set; } = string.Empty;

        [JsonProperty("versions__delta")]
        public string VersionsDelta
        {
            get { return _VersionsDelta.ToString(); }
            set { _VersionsDelta = Convert.ToInt32(value); }
        }

        [JsonIgnore]
        public int _VersionsDelta { get; set; } = 1;

        [JsonProperty("apps__hashcode")]
        public string AppsHashCode { get; set; } = string.Empty;

        [JsonProperty("application")]
        public List<Application> Applications { get; set; } = new List<Application>();
    }
}

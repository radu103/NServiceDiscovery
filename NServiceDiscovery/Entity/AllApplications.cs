﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace NServiceDiscovery.Entity
{
    public class AllApplications
    {
        [JsonProperty("versions__delta")]
        public long VersionsDelta { get; set; }

        [JsonProperty("apps__hashcode")]
        public string AppsHashCode { get; set; } = string.Empty;

        [JsonProperty("application")]
        public List<Application> Applications { get; set; } = new List<Application>();
    }
}

﻿using Newtonsoft.Json;
using NServiceDiscovery.Entity;
using System.Collections.Generic;

namespace NServiceDiscovery.RuntimeInMemory
{
    public class ConfigurationStore
    {
        [JsonProperty("generalKeyValues")]
        public List<GeneralKeyValue> GeneralKeyValues = new List<GeneralKeyValue>();
    }
}

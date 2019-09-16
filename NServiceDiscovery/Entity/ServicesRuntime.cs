using System.Collections.Generic;

namespace NServiceDiscovery.Entity
{
    public class ServicesRuntime
    {
        // "versions__deltam"
        public string VersionsDelta { get; set; } = "1";

        //"apps__hashcode": "UP_2_"
        public string AppsHashCode { get; set; } = "";

        // "application" : []
        public static List<ServiceApplication> Applications = new List<ServiceApplication>();
    }
}

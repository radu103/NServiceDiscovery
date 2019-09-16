using System;

namespace NServiceDiscovery
{
    public class ConfigurationHelper
    {
        public String TenantID { get; set; }

        public static ConfigurationData Load()
        {
            var conf = new ConfigurationData(){
                TenantID = "public",
                OwnBaseHref = "https://localhost:44334"
            };

            return conf;
        }
    }
}

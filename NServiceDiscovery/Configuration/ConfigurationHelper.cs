using NServiceDiscovery.Configuration;
using System;

namespace NServiceDiscovery
{
    public class ConfigurationHelper
    {
        public String TenantID { get; set; }

        public static ConfigurationData Load()
        {
            var conf = new ConfigurationData(){
                TenantID = DefaultConfigurationData.DefaultTenantID,
                OwnBaseHref = DefaultConfigurationData.DefaultOwnBaseHref
            };

            return conf;
        }
    }
}

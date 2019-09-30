using Newtonsoft.Json;
using NServiceDiscovery.Common.CloudFoundry;
using System;

namespace NServiceDiscovery.Util
{
    public class CloudFoundryEnvironmentUtil
    {
        public static CloudFoundryVcapApplication GetApplicationFromEnv()
        {
            CloudFoundryVcapApplication app = null;

            // get VCAP_APPLICATION
            try
            {
                var VCAP_APPLICATION = Environment.GetEnvironmentVariable("VCAP_APPLICATION");
                Console.WriteLine("VCAP_APPLICATION : " + VCAP_APPLICATION);

                if (VCAP_APPLICATION != null)
                {
                    var vcapApp = JsonConvert.DeserializeObject<CloudFoundryVcapApplication>(VCAP_APPLICATION, NServiceDiscoveryJsonSerializerSettings.IgnoreMissingPropetiesSettings);
                    app = vcapApp;
                }
            }
            catch (Exception err)
            {
                app = null;
                Console.WriteLine("VCAP_APPLICATION ERROR : " + err.Message);
            }

            return app;
        }

        public static CloudFoundryVcapServices GetServicesFromEnv()
        {
            CloudFoundryVcapServices services = null;

            // get VCAP_SERVICES
            try
            {
                var VCAP_SERVICES = Environment.GetEnvironmentVariable("VCAP_SERVICES");
                Console.WriteLine("VCAP_SERVICES : " + VCAP_SERVICES);

                if (VCAP_SERVICES != null)
                {
                    var allVcapServices = JsonConvert.DeserializeObject<CloudFoundryVcapServices>(VCAP_SERVICES, NServiceDiscoveryJsonSerializerSettings.IgnoreMissingPropetiesSettings);
                    services = allVcapServices;
                }
            }
            catch (Exception err)
            {
                services = null;
                Console.WriteLine("VCAP_SERVICES ERROR : " + err.Message);
            }

            return services;
        }

        public static string GetInstanceGuidFromEnv()
        {
            string instanceGuid = string.Empty;

            // get CF_INSTANCE_GUID
            try
            {
                var CF_INSTANCE_GUID = Environment.GetEnvironmentVariable("CF_INSTANCE_GUID");

                Console.WriteLine("CF_INSTANCE_GUID : " + CF_INSTANCE_GUID);

                instanceGuid = CF_INSTANCE_GUID;
            }
            catch (Exception err)
            {
                Console.WriteLine("CF_INSTANCE_GUID ERROR : " + err.Message);
            }

            return instanceGuid;
        }

        public static string GetInstanceIpFromEnv()
        {
            string instanceIp = string.Empty;

            // get CF_INSTANCE_IP if available
            try
            {
                var CF_INSTANCE_IP = Environment.GetEnvironmentVariable("CF_INSTANCE_IP");

                Console.WriteLine("CF_INSTANCE_IP : " + CF_INSTANCE_IP);

                instanceIp = CF_INSTANCE_IP;
            }
            catch (Exception err)
            {
                Console.WriteLine("CF_INSTANCE_GUID/IP/PORT ERROR : " + err.Message);
            }

            return instanceIp;
        }

        public static int GetInstancePortFromEnv()
        {
            int instancePort = 0;

            // get CF_INSTANCE_PORT if available
            try
            {
                var CF_INSTANCE_PORT = Environment.GetEnvironmentVariable("CF_INSTANCE_PORT");

                Console.WriteLine("CF_INSTANCE_PORT : " + CF_INSTANCE_PORT);

                instancePort = Convert.ToInt32(CF_INSTANCE_PORT);
            }
            catch (Exception err)
            {
                Console.WriteLine("CF_INSTANCE_GUID/IP/PORT ERROR : " + err.Message);
            }

            return instancePort;
        }

        public static string GetTenantIdFromEnv()
        {
            string tenantId = string.Empty;

            // get TENANT_ID, TENANT_TYPE
            try
            {
                var TENANT_ID = Environment.GetEnvironmentVariable("SINGLE_TENANT_ID");

                Console.WriteLine("SINGLE_TENANT_ID : " + TENANT_ID);

                tenantId = TENANT_ID;
            }
            catch (Exception err)
            {
                Console.WriteLine("TENANT_ID ERROR : " + err.Message);
            }

            return tenantId;
        }

        public static string GetTenantTypeFromEnv()
        {
            string tenatType = string.Empty;

            // get TENANT_ID, TENANT_TYPE
            try
            {
                var TENANT_TYPE = Environment.GetEnvironmentVariable("SINGLE_TENANT_TYPE");

                Console.WriteLine("SINGLE_TENANT_TYPE : " + TENANT_TYPE);

                tenatType = TENANT_TYPE;
            }
            catch (Exception err)
            {
                Console.WriteLine("TENANT_TYPE ERROR : " + err.Message);
            }

            return tenatType;
        }
    }
}

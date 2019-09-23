using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using NServiceDiscovery;
using NServiceDiscovery.Entity;
using NServiceDiscoveryAPI.Services;
using System;

namespace NServiceDiscoveryAPI
{
    public class Program
    {
        public static string SINGLE_TENANT_ID = string.Empty;
        public static string SINGLE_TENANT_TYPE = string.Empty;

        public static CloudFoundryVcapApplication cloudFoundryVcapApplication;

        public static ConfigurationData InstanceConfig;

        public static IMQTTService mqttService;
        public static IEvictionService evictionService;

        private static void GetCFEnv()
        {
            // get VCAP_APPLICATION
            try
            {
                var VCAP_APPLICATION = Environment.GetEnvironmentVariable("VCAP_APPLICATION");
                Console.WriteLine("VCAP_APPLICATION : " + VCAP_APPLICATION);

                if (VCAP_APPLICATION != null)
                {
                    var vcapApp = JsonConvert.DeserializeObject<CloudFoundryVcapApplication>(VCAP_APPLICATION);
                    Program.cloudFoundryVcapApplication = vcapApp;
                }
            }
            catch (Exception err)
            {
                Program.cloudFoundryVcapApplication = null;
                Console.WriteLine("VCAP_APPLICATION ERROR : " + err.Message);
            }

            // get CF_INSTANCE_GUID, CF_INSTANCE_IP, CF_INSTANCE_PORT if available
            try
            {
                var CF_INSTANCE_GUID = Environment.GetEnvironmentVariable("CF_INSTANCE_GUID");
                var CF_INSTANCE_IP = Environment.GetEnvironmentVariable("CF_INSTANCE_IP");
                var CF_INSTANCE_PORT = Environment.GetEnvironmentVariable("CF_INSTANCE_PORT");

                Console.WriteLine("CF_INSTANCE_GUID : " + CF_INSTANCE_GUID);
                Console.WriteLine("CF_INSTANCE_IP : " + CF_INSTANCE_GUID);
                Console.WriteLine("CF_INSTANCE_PORT : " + CF_INSTANCE_GUID);

                Program.cloudFoundryVcapApplication.InstanceGuid = CF_INSTANCE_GUID;
                Program.cloudFoundryVcapApplication.InstanceIP = CF_INSTANCE_IP;
                Program.cloudFoundryVcapApplication.InstancePort = Convert.ToInt32(CF_INSTANCE_PORT);
            }
            catch (Exception err)
            {
                Console.WriteLine("CF_INSTANCE_GUID/IP/PORT ERROR : " + err.Message);
            }
            
            // get TENANT_ID, TENANT_TYPE
            try
            {
                var TENANT_ID = Environment.GetEnvironmentVariable("SINGLE_TENANT_ID");
                var TENANT_TYPE = Environment.GetEnvironmentVariable("SINGLE_TENANT_TYPE");

                Console.WriteLine("SINGLE_TENANT_ID : " + TENANT_ID);
                Console.WriteLine("SINGLE_TENANT_ID : " + TENANT_TYPE);

                Program.SINGLE_TENANT_ID = TENANT_ID;
                Program.SINGLE_TENANT_TYPE = TENANT_TYPE;
            }
            catch (Exception err)
            {
                Console.WriteLine("TENANT_ID/TENANT_TYPE ERROR : " + err.Message);
            }
        }

        public static void Main(string[] args)
        {
            GetCFEnv();

            Program.InstanceConfig = ConfigurationHelper.Load(cloudFoundryVcapApplication);

            if(Program.InstanceConfig != null && Program.cloudFoundryVcapApplication != null && Program.cloudFoundryVcapApplication.ApplicationUrls.Count > 0)
            {
                Program.InstanceConfig.Urls = Program.cloudFoundryVcapApplication.ApplicationUrls[0];
            }

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseIISIntegration().UseStartup<Startup>();
    }
}

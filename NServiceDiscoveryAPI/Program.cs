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
        public static CloudFoundryVcapApplication cloudFoundryVcapApplication;
        public static ConfigurationData InstanceConfig;

        public static IMQTTService mqttService;
        public static IEvictionService evictionService;

        private static void GetCFEnv()
        {
            try
            {
                var VCAP_APPLICATION = Environment.GetEnvironmentVariable("VCAP_APPLICATION");

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
        }

        public static void Main(string[] args)
        {
            GetCFEnv();

            Program.InstanceConfig = ConfigurationHelper.Load(cloudFoundryVcapApplication);

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseIISIntegration().UseStartup<Startup>();
    }
}

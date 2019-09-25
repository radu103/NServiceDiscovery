using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NServiceDiscovery;
using NServiceDiscovery.CloudFoundry;
using NServiceDiscovery.Util;
using NServiceDiscoveryAPI.Services;

namespace NServiceDiscoveryAPI
{
    public class Program
    {
        public static string SINGLE_TENANT_ID = string.Empty;
        public static string SINGLE_TENANT_TYPE = string.Empty;

        public static string INSTANCE_GUID = string.Empty;
        public static string INSTANCE_IP = string.Empty;
        public static int INSTANCE_PORT = 0;

        public static CloudFoundryVcapApplication cloudFoundryVcapApplication;
        public static CloudFoundryVcapServices cloudFoundryVcapServices;

        public static ConfigurationData InstanceConfig;

        public static IMQTTService mqttService;
        public static IEvictionService evictionService;

        public static void Main(string[] args)
        {
            Program.SINGLE_TENANT_ID = CloudFoundryEnvironmentUtil.GetTenantIdFromEnv();
            Program.SINGLE_TENANT_TYPE = CloudFoundryEnvironmentUtil.GetTenantTypeFromEnv();

            Program.INSTANCE_GUID = CloudFoundryEnvironmentUtil.GetInstanceGuidFromEnv();
            Program.INSTANCE_IP = CloudFoundryEnvironmentUtil.GetInstanceIpFromEnv();
            Program.INSTANCE_PORT = CloudFoundryEnvironmentUtil.GetInstancePortFromEnv();

            Program.cloudFoundryVcapApplication = CloudFoundryEnvironmentUtil.GetApplicationFromEnv();
            Program.cloudFoundryVcapServices = CloudFoundryEnvironmentUtil.GetServicesFromEnv();

            Program.InstanceConfig = ConfigurationHelper.Load(cloudFoundryVcapApplication, Program.INSTANCE_GUID);

            if(Program.InstanceConfig != null && Program.cloudFoundryVcapApplication != null && Program.cloudFoundryVcapApplication.ApplicationUrls.Count > 0)
            {
                Program.InstanceConfig.Urls = Program.cloudFoundryVcapApplication.ApplicationUrls[0];
            }

            if (!string.IsNullOrEmpty(Program.SINGLE_TENANT_ID))
            {
                Program.InstanceConfig.MQTTTopicName = "NServiceDiscovery-" + Program.SINGLE_TENANT_ID + "-" + Program.SINGLE_TENANT_TYPE;
            }

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseIISIntegration().UseStartup<Startup>();
    }
}

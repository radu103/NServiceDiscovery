using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using NServiceDiscovery;
using NServiceDiscovery.Common.CloudFoundry;
using NServiceDiscovery.Entity;
using NServiceDiscovery.Persistency;
using NServiceDiscovery.Repository;
using NServiceDiscovery.Util;
using NServiceDiscoveryAPI.Services;
using System;
using System.Collections.Generic;
using System.Timers;

namespace NServiceDiscoveryAPI
{
    public class Program
    {
        public static IServiceProvider serviceProvider;
        public static List<Tenant> Tenants = new List<Tenant>();

        public static string INSTANCE_GUID = string.Empty;
        public static string INSTANCE_IP = string.Empty;
        public static int INSTANCE_PORT = 0;

        public static IMongoDBSettings mongoDbSettings;

        public static CloudFoundryVcapApplication cloudFoundryVcapApplication;
        public static CloudFoundryVcapServices cloudFoundryVcapServices;

        public static ConfigurationData InstanceConfig;

        public static IMQTTService mqttService;
        public static IEvictionService evictionService;
        public static IPersistencyService persistencyService;

        private static Timer _gcTimer;

        private static void OnGCTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("GC started at {0:HH:mm:ss.fff}", e.SignalTime);
            System.GC.Collect();
        }

        private static void StartGCTimer()
        {
            _gcTimer = new System.Timers.Timer(10 * 60 * 1000); // 10 minutes
            _gcTimer.AutoReset = true;
            _gcTimer.Enabled = true;
            _gcTimer.Elapsed += OnGCTimedEvent;
        }

        public static void Main(string[] args)
        {
            StartGCTimer();

            var SINGLE_TENANT_ID = CloudFoundryEnvironmentUtil.GetTenantIdFromEnv();
            var SINGLE_TENANT_TYPE = CloudFoundryEnvironmentUtil.GetTenantTypeFromEnv();

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

            if (!string.IsNullOrEmpty(SINGLE_TENANT_ID) && !string.IsNullOrEmpty(SINGLE_TENANT_TYPE))
            {
                Tenant singleTenant = new Tenant()
                {
                    TenantId = SINGLE_TENANT_ID
                };

                Tenants.Add(singleTenant);
            }
            else
            {
                var repoTenants = new MemoryTenantsRepository();
                Tenants = repoTenants.GetAll();
            }

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseIISIntegration().UseStartup<Startup>();
    }
}

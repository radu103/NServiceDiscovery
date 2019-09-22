using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NServiceDiscovery;
using NServiceDiscoveryAPI.Services;

namespace NServiceDiscoveryAPI
{
    public class Program
    {
        public static ConfigurationData InstanceConfig;

        public static IMQTTService mqttService;

        public static void Main(string[] args)
        {
            Program.InstanceConfig = ConfigurationHelper.Load();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}

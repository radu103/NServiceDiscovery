using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NServiceDiscovery;

namespace NServiceDiscoveryAPI
{
    public class Program
    {
        public static ConfigurationData config;

        public static void Main(string[] args)
        {
            Program.config = ConfigurationHelper.Load();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}

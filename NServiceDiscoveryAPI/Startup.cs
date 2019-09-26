using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceDiscovery.Repository;
using NServiceDiscoveryAPI.GlobalFilters;
using NServiceDiscoveryAPI.Services;
using System.IO.Compression;

namespace NServiceDiscoveryAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                config.Filters.Add(new CopyTenantIdFromBearerTokentoRouteDataFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddSingleton<IMQTTService, MQTTService>();
            services.AddSingleton<IInstanceStatusService, InstanceStatusService>();
            services.AddSingleton<IEvictionService, EvictionService>();
            services.AddSingleton<IMemoryDiscoveryPeerRepository, MemoryDiscoveryPeerRepository>();
            services.AddSingleton<IPersistencyRepository, PersistencyRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseMvc();

            StartupOps(app);
        }

        private void StartupOps(IApplicationBuilder app)
        {
            // instantiate the MQTTService singleton instance
            var serviceProvider = app.ApplicationServices;
            Program.mqttService = serviceProvider.GetService<IMQTTService>();
            Program.evictionService = serviceProvider.GetService<IEvictionService>();
        }
    }
}

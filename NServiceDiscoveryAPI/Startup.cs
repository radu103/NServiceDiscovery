using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceDiscovery.Persistency;
using NServiceDiscovery.Repository;
using NServiceDiscoveryAPI.GlobalFilters;
using NServiceDiscoveryAPI.Services;
using System;
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
            services.AddSingleton<IMQTTProcessingService, MQTTProcessingService>();

            services.AddSingleton<IPublishChangesService, PublishChangesService>();
            services.AddSingleton<IPublishClientsService, PublishClientsService>();

            services.AddSingleton<IEvictionService, EvictionService>();
            services.AddSingleton<IMemoryTenantsRepository, MemoryTenantsRepository>();
            services.AddSingleton<IPersistencyService, PersistencyService>();

            services.AddSingleton<IMemoryDiscoveryPeerRepository, MemoryDiscoveryPeerRepository>(); 
            services.AddSingleton<IMemoryDiscoveryClientRepository, MemoryDiscoveryClientRepository>();
            services.AddSingleton<IMemoryGeneralConfigurationClientRepository, MemoryGeneralConfigurationClientRepository>();

            services.AddSingleton<IInstanceStatusService, InstanceStatusService>();
            services.AddSingleton<IInstanceHealthService, InstanceHealthService>();

            // persistency repositories
            services.AddSingleton<IMongoDBSettings, MongoDBSettings>();

            services.AddSingleton<IPersistencyDefaultApplicationsRepository, PersistencyDefaultApplicationsRepository>();
            services.AddSingleton<IPersistencyTenantRepository, PersistencyTenantRepository>();

            services.AddSingleton<IPersistencyAppConfigurationsRepository, PersistencyAppConfigurationsRepository>();
            services.AddSingleton<IPersistencyGeneralConfigurationsRepository, PersistencyGeneralConfigurationsRepository>();
            services.AddSingleton<IPersistencyApplicationsRepository, PersistencyApplicationsRepository>();
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
            
            app.UseStaticFiles(new StaticFileOptions()
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "text/plain"
            });

            StartupOps(app);
        }

        private void StartupOps(IApplicationBuilder app)
        {
            // instantiate the MQTTService singleton instance
            Program.serviceProvider = app.ApplicationServices;

            Program.mqttService = Program.serviceProvider.GetService<IMQTTService>();
            Program.evictionService = Program.serviceProvider.GetService<IEvictionService>();
            Program.persistencyService = Program.serviceProvider.GetService<IPersistencyService>();

            Program.mqttService = Program.serviceProvider.GetService<IMQTTService>();

            // get mongo db configuration
            Program.mongoDbSettings = Program.serviceProvider.GetService<IMongoDBSettings>();
            Program.mongoDbSettings.MongoDbUrl = "mongodb://admin:admin@ds235711.mlab.com:35711/nservicediscovery";
            Program.mongoDbSettings.User = "admin";
            Program.mongoDbSettings.Password = "admin";
            Program.mongoDbSettings.HostName = "ds235711.mlab.com";
            Program.mongoDbSettings.Port = 35711;
            Program.mongoDbSettings.DbName = "nservicediscovery";

            // TO DO : get mongo db configuration from CF environment vars

            // start persistency sync timer with random interval
            var random = new Random();
            Program.persistencyService.SetPersistencyTimerInterval(random.Next(Program.InstanceConfig.PersistencySyncMinRandomSeconds, Program.InstanceConfig.PersistencySyncMaxRandomSeconds));
        }
    }
}

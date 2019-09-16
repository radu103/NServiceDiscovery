using System.Collections.Generic;
using NServiceDiscovery.Entity;
using System.Linq;
using NServiceDiscovery.RuntimeInMemory;
using NServiceDiscovery.Configuration;
using System;

namespace NServiceDiscovery.Repository
{
    public class MemoryServicesRepository
    {
        private string repoTenantId = DefaultConfigurationData.DefaultTenantID;

        public MemoryServicesRepository(string tenantId)
        {
            if (!string.IsNullOrEmpty(tenantId))
            {
                repoTenantId = tenantId;
            }
        }

        public ServicesRuntime GetAll()
        {
            return Memory.Runtime;
        }

        public ServiceInstance GetByInstanceId(string instanceId)
        {
            ServiceInstance instance = null;

            foreach (var app in ServicesRuntime.Applications)
            {
                instance = app.Instances.SingleOrDefault(i => i.TenantId.CompareTo(repoTenantId) == 0 && i.InstanceId.CompareTo(instanceId) == 0);
                if(instance != null) { break; }
            }

            return instance;
        }

        public List<ServiceInstance> GetByAppName(string appName)
        {
            var list = new List<ServiceInstance>();

            var app = ServicesRuntime.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if (app != null)
            {
                list = app.Instances.FindAll(i => i.TenantId.CompareTo(repoTenantId) == 0).ToList();
            }

            return list;
        }

        public bool ChangeStatus(string appName, string instanceId, string status)
        {
            var app = ServicesRuntime.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if (app != null)
            {
                for(var i = 0; i < app.Instances.Count; i++)
                {
                    if(app.Instances[i].TenantId.CompareTo(repoTenantId) == 0 && app.Instances[i].InstanceId.CompareTo(instanceId) == 0)
                    {
                        app.Instances[i].Status = status;
                        break;
                    }
                }

                return true;
            }

            return false;
        }

        public bool Delete(string appName, string instanceId)
        {
            ServiceInstance instance = null;

            var app = ServicesRuntime.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if(app != null)
            {
                instance = app.Instances.SingleOrDefault(i => i.TenantId.CompareTo(repoTenantId) == 0 && i.InstanceId.CompareTo(instanceId) == 0);

                if (instance != null) {
                    app.Instances.Remove(instance);
                    return true;
                }
            }

            return false;
        }

        public ServiceInstance Add(ServiceInstance instance)
        {
            var appId = instance.AppName;

            var appFound = ServicesRuntime.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appId) == 0);
            if(appFound == null)
            {
                appFound = new ServiceApplication()
                {
                    Name = appId,
                    Protocol = ApplicationProtocol.HTTP,
                    Instances = new List<ServiceInstance>()
                };

                ServicesRuntime.Applications.Add(appFound);
            }

            appFound.Instances.Add(instance);

            return instance;
        }
    }
}

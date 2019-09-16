using System;
using System.Collections.Generic;
using NServiceDiscovery.Entity;
using System.Linq;
using NServiceDiscovery.RuntimeInMemory;

namespace NServiceDiscovery.Repository
{
    public class ServiceRepository
    {
        public ServicesRuntime GetAll()
        {
            return Memory.Runtime;
        }

        public ServiceInstance GetByInstanceId(string instanceId)
        {
            ServiceInstance instance = null;

            foreach (var app in ServicesRuntime.Applications)
            {
                instance = app.Instances.SingleOrDefault(i => i.InstanceId.CompareTo(instanceId) == 0);
                if(instance != null) { break; }
            }

            return instance;
        }

        public List<ServiceInstance> GetByAppName(string appName)
        {
            var list = new List<ServiceInstance>();

            var app = ServicesRuntime.Applications.SingleOrDefault(a => a.Name.CompareTo(appName) == 0);

            if (app != null)
            {
                list = app.Instances;
            }

            return list;
        }

        public bool Delete(string instanceId)
        {
            ServiceInstance instance = null;

            foreach (var app in ServicesRuntime.Applications)
            {
                instance = app.Instances.SingleOrDefault(i => i.InstanceId.CompareTo(instanceId) == 0);

                if (instance != null) {
                    app.Instances.Remove(instance);
                    break;
                }
            }

            return true;
        }

        public ServiceInstance Add(ServiceInstance instance)
        {
            var appId = instance.AppId;

            var appFound = ServicesRuntime.Applications.SingleOrDefault(a => a.Name.CompareTo(appId) == 0);
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

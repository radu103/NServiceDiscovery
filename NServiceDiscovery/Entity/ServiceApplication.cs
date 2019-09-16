using System;
using System.Collections.Generic;
using System.Text;

namespace NServiceDiscovery.Entity
{
    public class ServiceApplication
    {
        // "name" : "APPID_1"
        public string Name { get; set; } = string.Empty;

        public ApplicationProtocol Protocol { get; set; } = ApplicationProtocol.HTTP;

        // "instance" : []
        public List<ServiceInstance> Instances = new List<ServiceInstance>();
    }
}

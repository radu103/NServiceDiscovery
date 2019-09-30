using Newtonsoft.Json;

namespace NServiceDiscovery.Common.CloudFoundry
{
    public class CloudFoundryVcapServiceCredentials
    {
        [JsonProperty("uri")]
        public string URI { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("dbname")]
        public string DbName { get; set; }

        [JsonProperty("port")]
        public string Port { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }
    }
}

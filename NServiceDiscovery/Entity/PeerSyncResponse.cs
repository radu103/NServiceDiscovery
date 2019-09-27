using Newtonsoft.Json;

namespace NServiceDiscovery.Entity
{
    public class PeerSyncResponse
    {
        [JsonProperty("peerId")]
        public string PeerId { get; set; }

        [JsonProperty("md5Hash")]
        public string Md5Hash { get; set; }
    }
}

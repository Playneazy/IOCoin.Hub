using IOCoin.Headless.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Responses
{
    public class AliasResponse
    {
        [JsonProperty("alias")]
        public string Alias { get; set; }
        [JsonProperty("encrypted")]
        public bool Encrypted { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("nHeigt")]
        public long Height { get; set; }
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }
        [JsonProperty("expired")]
        public int Expired { get; set; }
        [JsonProperty("tunnel_switch")]
        public long TunnelSwitch { get; set; }
        [JsonProperty("status")]
        public long Status { get; set; }
        [JsonProperty("xtu")]
        public long xTu { get; set; }
    }
}

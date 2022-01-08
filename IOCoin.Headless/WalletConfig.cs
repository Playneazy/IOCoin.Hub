using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless
{
    public class WalletConfig
    {
        [JsonProperty("configname")]
        public string WalletName { get; set; }
        [JsonProperty]
        public string daemonPath { get; set; }
        [JsonProperty]
        public string appDataDir { get; set; }
        [JsonProperty]
        public string configFilepath { get; set; }

        [JsonIgnore]
        public string rpcUser { get; set; } = "iocoinrpc";
        [JsonIgnore]
        public string rpcPassword { get; set; }

        [JsonIgnore]
        public string daemonArgBase { get; set; }
        [JsonProperty]
        public string walletPassphrase { get; set; }
        [JsonProperty]
        public string notificationAddress { get; set; }

        [JsonProperty("initnodes")]
        public IEnumerable<string> addNodes { get; set; } = new List<string>();
    }
}

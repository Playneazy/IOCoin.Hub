using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless
{
    public class Settings
    {
        [JsonProperty("configname")]
        public string WalletName { get; set; }
        [JsonProperty]
        public string daemonPath { get; set; }
        [JsonProperty]
        public string appDataDir { get; set; }
        [JsonProperty]
        public string configFilepath { get; set; }

        [JsonProperty]
        public string rpcUser { get; set; } = "iocoinrpc";
        [JsonProperty]
        public string rpcPassword { get; set; }

        [JsonProperty]
        public string daemonArgBase { get; set; }
        [JsonProperty]
        public string walletPasshrase { get; set; }
        [JsonProperty]
        public string notificationAddress { get; set; }



        [JsonProperty]
        public IEnumerable<string> addNodes { get; set; } = new List<string>();


        

    }
}

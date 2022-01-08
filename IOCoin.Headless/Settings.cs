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
        [JsonProperty]
        public string lastusedwallet { get; set; }

        [JsonProperty]
        public List<WalletConfig> walletconfigs { get; set; } 

    }
}

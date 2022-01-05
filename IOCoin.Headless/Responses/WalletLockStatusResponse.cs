using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Responses
{
    public class WalletLockStatusResponse
    {
        [JsonProperty("isEncrypted")]
        public bool isEncrypted { get; set; }
        [JsonProperty("isLocked")]
        public bool isLocked { get; set; }
    }
}

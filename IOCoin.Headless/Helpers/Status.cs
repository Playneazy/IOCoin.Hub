using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Helpers
{
    public class Status
    {
        [JsonProperty]
        public bool isOnline { get; set; } = false;
        [JsonProperty]
        public bool isSynced { get; set; } = false;
        [JsonProperty]
        public bool isEncrypted { get; set; } = false;
        [JsonProperty]
        public bool isLocked { get; set; } = false;
        [JsonProperty]
        public bool isStaking { get; set; } = false;
    }
}

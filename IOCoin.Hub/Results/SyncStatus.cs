using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Console.Logic.Results
{
    public class SyncStatus
    {
        [JsonProperty]
        public int BlockCount { get; set; }
        [JsonProperty]
        public int BlockCountOfPeers { get; set; }
        [JsonProperty]
        public int Difference { get; set; }
    }
}

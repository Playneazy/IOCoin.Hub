using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Responses
{
    public class GetInfoResponse
    {
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("protocolversion")]
        public int ProtocolVersion { get; set; }
        [JsonProperty("walletversion")]
        public int WalletVersion { get; set; }
        [JsonProperty("balance")]
        public decimal Balance { get; set; }
        [JsonProperty("pending")]
        public decimal Pending { get; set; }
        [JsonProperty("newmint")]
        public decimal NewMint { get; set; }
        [JsonProperty("stake")]
        public decimal Stake { get; set; }
        [JsonProperty("blocks")]
        public int Blocks { get; set; }
        [JsonProperty("powblocks")]
        public int PowBlocks { get; set; }
        [JsonProperty("powblocksleft")]
        public int PowBlocksLeft { get; set; }
        [JsonProperty("timeoffset")]
        public int TimeOffset { get; set; }
        [JsonProperty("connections")]
        public int Connections { get; set; }
        [JsonProperty("proxy")]
        public string Proxy { get; set; }
        [JsonProperty("ip")]
        public string IP { get; set; }
        [JsonProperty("difficulty")]
        public DifficultyResponse Difficulty { get; set; }
        [JsonProperty("testnet")]
        public bool TestNet { get; set; }
        [JsonProperty("keypoololdest")]
        public int KeyPoolOldest { get; set; }
        [JsonProperty("keypoolsize")]
        public int KeyPoolSize { get; set; }
        [JsonProperty("paytxfee")]
        public decimal PayTxFee { get; set; }
        [JsonProperty("mininput")]
        public decimal MinInput { get; set; }
        [JsonProperty("unlocked_until")]
        public decimal UnlockedUntil { get; set; }
        [JsonProperty("errors")]
        public string Errors { get; set; }
    }
}

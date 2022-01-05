using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Responses
{
    public class GetStakingInfoResponse
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
        [JsonProperty("staking")]
        public bool Staking { get; set; }
        [JsonProperty("errors")]
        public string Errors { get; set; }
        [JsonProperty("currentblocksize")]
        public int CurrentBlockSize { get; set; }
        [JsonProperty("currentblocktx")]
        public int CurrentBlockTx { get; set; }
        [JsonProperty("pooledtx")]
        public bool PooledTx { get; set; }
        [JsonProperty("difficulty")]
        public decimal Difficulty { get; set; }
        [JsonProperty("search-interval")]
        public int SearchInterval { get; set; }
        [JsonProperty("weight")]
        public UInt64 Weight { get; set; }
        [JsonProperty("netstakeweight")]
        public UInt64 NetStakeWeight { get; set; }
        [JsonProperty("expectedtime")]
        public UInt64 ExpectedTime { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Responses
{
    public class DifficultyResponse
    {
        [JsonProperty("proof-of-work")]
        public decimal ProofOfWork { get; set; }
        [JsonProperty("proof-of-stake")]
        public decimal ProofOfStake { get; set; }
    }
}

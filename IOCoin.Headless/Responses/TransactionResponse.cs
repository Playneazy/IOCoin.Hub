using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Responses
{
    public class TransactionResponse
    {
        [JsonProperty("txid")]
        public string TxId { get; set; }
        [JsonProperty("version")]
        public long Version { get; set; }
        [JsonProperty("time")]
        public ulong Time { get; set; }
        [JsonProperty("locktime")]
        public ulong LockTime { get; set; }


        [JsonProperty("vin")]
        public List<TransactionVIN> vINs { get; set; }

        [JsonProperty("vout")]
        public List<TransactionVOUT> vOUTs { get; set; }



        [JsonProperty("blockhash")]
        public string BlockHash { get; set; }
        [JsonProperty("confirmations")]
        public ulong Confirmations { get; set; }
    }
}

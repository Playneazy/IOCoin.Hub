using IOCoin.Headless.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Responses
{
    public class ListTransactionsResponse
    {
        [JsonProperty("account")]
        public string Account { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("amount")]
        public decimal amount { get; set; }
        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }
        [JsonProperty("blockhash")]
        public string BlockHash { get; set; }
        [JsonProperty("blockindex")]
        public int BlockIndex { get; set; }
        [JsonProperty("blocktime")]
        public long BlockTime { get; set; }
        [JsonProperty("txid")]
        public string TxId { get; set; }
        [JsonProperty("time")]
        public long Time { get; set; }
        [JsonProperty("tx-info")]
        public string TxInfo { get; set; }
        [JsonProperty("timereceived")]
        public long TimeReceived { get; set; }
    }
}

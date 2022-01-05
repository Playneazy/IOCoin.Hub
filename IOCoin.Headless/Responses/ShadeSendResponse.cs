using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Responses
{
    public class ShadeSendResponse
    {
        [JsonProperty("abs")]
        public string Abs { get; set; }
        [JsonProperty("ord")]
        public string Ord { get; set; }
        [JsonProperty("target")]
        public string Target { get; set; }
        [JsonProperty("trace")]
        public string Trace { get; set; }
        [JsonProperty("txid")]
        public string TxId { get; set; }
    }
}

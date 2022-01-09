using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Responses
{
    public class TransactionVOUT
    {
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("n")]
        public long N { get; set; }
        
        [JsonProperty("scriptPubKey")]
        public ScriptPubKey ScriptPublicKey { get; set; }
        
  


        public class ScriptPubKey
        {
            [JsonProperty("asm")]
            public string Asm { get; set; }
            [JsonProperty("reqSigs")]
            public long ReqSigs { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("addresses")]
            public string[] Addresses { get; set; }
        }
    }
}

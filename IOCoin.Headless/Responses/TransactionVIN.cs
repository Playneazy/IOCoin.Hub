using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Responses
{
    public class TransactionVIN
    {
        [JsonProperty("txid")]
        public string TxId { get; set; }
        [JsonProperty("vout")]
        public long vOut { get; set; }
        
        [JsonProperty("scriptSig")]
        public ScriptSig ScriptSignature { get; set; }
        
        [JsonProperty("sequence")]
        public ulong Sequence { get; set; }


        public class ScriptSig
        {
            [JsonProperty("asm")]
            public string Asm { get; set; }
            [JsonProperty("hex")]
            public string Hex { get; set; }
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Responses
{
    public class GetBlockResponse
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }
        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }
        [JsonProperty("size")]
        public long Size { get; set; }
        [JsonProperty("height")]
        public long Height { get; set; }
        [JsonProperty("version")]
        public int Version { get; set; }
        [JsonProperty("merkleroot")]
        public string MerkleRoot { get; set; }
        [JsonProperty("mint")]
        public decimal Mint { get; set; }
        [JsonProperty("time")]
        public long Time { get; set; }
        [JsonProperty("nonce")]
        public int Nonce { get; set; }
        [JsonProperty("bits")]
        public string Bits { get; set; }
        [JsonProperty("difficulty")]
        public decimal Difficulty { get; set; }
        [JsonProperty("blocktrust")]
        public string BlockTrust { get; set; }
        [JsonProperty("chaintrust")]
        public string ChainTrust { get; set; }
        [JsonProperty("previousblockhash")]
        public string PreviousBlockHash { get; set; }
        [JsonProperty("flags")]
        public string Flags { get; set; }
        [JsonProperty("proofhash")]
        public string ProofHash { get; set; }
        [JsonProperty("entropybit")]
        public long EntropyBit { get; set; }
        [JsonProperty("modifier")]
        public string Modifier { get; set; }
        [JsonProperty("modifierchecksum")]
        public string ModifierCheckSum { get; set; }
        [JsonProperty("tx")]
        public string[] Tx { get; set; }
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}

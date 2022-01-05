using IOCoin.Headless.Events;
using IOCoin.Headless.Helpers;
using IOCoin.Headless.Interfaces;
using IOCoin.Headless.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Wallet
{
    public class WalletBase : IWallet
    {
        public delegate void WalletUpdateEventHandler(object sender, WalletUpdateEventArgs e);
        public virtual void Update(WalletUpdateEventArgs e)
        {
            WalletUpdateEventHandler handler = WalletUpdate;
            handler?.Invoke(this, e);
        }
        public event WalletUpdateEventHandler WalletUpdate;



        [JsonProperty]
        public decimal Balance { get; set; } = 0;
        [JsonProperty]
        public decimal Pending { get; set; } = 0;
        [JsonProperty]
        public decimal NewMint { get; set; } = 0;
        [JsonProperty]
        public decimal Stake { get; set; } = 0;

        public List<ListTransactionsResponse> Transactions { get; set; } = new List<ListTransactionsResponse>();
        public List<AliasResponse> Aliases { get; set; } = new List<AliasResponse>();

        [JsonProperty]
        public string IP { get; set; }
        [JsonProperty]
        public long KeypoolOldest { get; set; } = 0;
        [JsonProperty]
        public long KeypoolSize { get; set; } = 0;

        [JsonProperty]
        public long BlockHeight { get; set; } = 0;
        [JsonProperty]
        public long PeerBlockCount { get; set; } = 0;

        [JsonProperty]
        public string Version { get; set; }
        [JsonProperty]
        public long ProtocolVersion { get; set; } = 0;
        [JsonProperty]
        public long WalletVersion { get; set; } = 0;

        [JsonProperty]
        public bool isOnline { get; set; } = false;
        [JsonProperty]
        public bool isEncrypted { get; set; } = false;
        [JsonProperty]
        public bool isLocked { get; set; } = true;
        [JsonProperty]
        public bool isStaking { get; set; } = false;

    }
}

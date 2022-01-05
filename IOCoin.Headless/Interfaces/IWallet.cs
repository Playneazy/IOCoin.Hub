using IOCoin.Headless.Events;
using IOCoin.Headless.Helpers;
using IOCoin.Headless.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Interfaces
{
    public interface IWallet
    {
        public delegate void WalletUpdateEventHandler(object sender, WalletUpdateEventArgs e);
        public void Update(WalletUpdateEventArgs e);

        public bool isOnline { get; set; }
        public bool isEncrypted { get; set; }
        public bool isLocked { get; set; }
        public bool isStaking { get; set; }


        public decimal Balance { get; set; }
        public decimal Pending { get; set; }
        public decimal NewMint { get; set; }
        public decimal Stake { get; set; }
        public List<ListTransactionsResponse> Transactions { get; set; }
        public List<AliasResponse> Aliases { get; set; }


        public string IP { get; set; }
        public long KeypoolOldest { get; set; }
        public long KeypoolSize { get; set; }

        public long BlockHeight { get; set; }
        public long PeerBlockCount { get; set; }

        public string Version { get; set; }
        public long ProtocolVersion { get; set; }
        public long WalletVersion { get; set; }
    }
}

using IOCoin.Headless.Helpers;
using IOCoin.Headless.Interfaces;
using IOCoin.Headless.Responses;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace IOCoin.Wallet
{
    public class Info : WalletBase
    {
        
        // Console app variables
        [JsonProperty]
        public bool isSynced { get; set; }
        [JsonProperty]
        public bool isWalletInitialized { get; set; }




    }
}

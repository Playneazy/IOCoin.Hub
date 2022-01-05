using IOCoin.Headless.Processes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Helpers
{
    public class WalletUpdateEventArgs
    {
        public WalletUpdateEventArgs(IWallet wallet) => Wallet = wallet;
        public IWallet Wallet { get; set; }
    }
}

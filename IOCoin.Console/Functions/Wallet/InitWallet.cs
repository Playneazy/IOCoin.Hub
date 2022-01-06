using IOCoin.Headless;
using IOCoin.Headless.Processes;
using IOCoin.Console.Helpers;
using IOCoin.Console.Logic;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOCoin.Wallet;

namespace IOCoin.Console.Functions.Wallet
{
    public class InitWallet
    {
        public async Task Run(Daemon Daemon, Info wallet, string walletName)
        {
            ConsoleWriter.Info($"Initializing wallet...");
            await new LoadWallet().Run(Daemon, wallet, walletName);
            wallet.isWalletInitialized = true;
            ConsoleWriter.Response($"Wallet [{walletName}] initialized.");
        }
    }
}

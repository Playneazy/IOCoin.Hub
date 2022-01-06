using IOCoin.Headless;
using IOCoin.Headless.Processes;
using IOCoin.Console.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOCoin.Wallet;

namespace IOCoin.Console.Functions
{
    public class Exit
    {
        public async Task Run(Daemon Daemon, Info wallet, string walletName)
        {
            ConsoleWriter.Important($"Stopping daemon and exiting...");

            await new Stop(Daemon.settings(walletName), wallet).Run();
            Environment.Exit(0);
        }
    }
}

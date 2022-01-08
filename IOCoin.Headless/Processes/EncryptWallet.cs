using IOCoin.Headless.Helpers;
using IOCoin.Headless.Interfaces;
using IOCoin.Headless.Responses;
using IOCoin.Headless.Responses.RPC;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Processes
{
    public class EncryptWallet : ProcessBase<bool>
    {
        public EncryptWallet(WalletConfig settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "encryptwallet";

        public override string CmdDesc => "Encrypts the wallet if not already encrypted. Uses passphrase from App.config key value 'walletpassphrase'.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<EncryptWallet> Run()
        {
            string args = $"{settings.walletPassphrase}";

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "encryptwallet " + args;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            await StartProcess(true, startInfo);

            // Create the Rpc.Result
            if (Rpc.ExitCode != 0)
            {
                await HandleResult(false, startInfo);
                return this;
            }

            if (Rpc.OutputMsg.StartsWith("wallet encrypted"))
            {
                // I/OCoin server stopping, restart to run with encrypted wallet.
                // The keypool has been flushed, you need to make a new backup.
                Log.Debug($"Wallet Encrypted with '{args}'.");
                Wallet.isEncrypted = true;
                await HandleResult(true, startInfo, true);
                return this;
            } else
            {
                await HandleResult(false, startInfo);
                return this;
            }

            
        }
    }
}

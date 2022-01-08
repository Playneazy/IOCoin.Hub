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
    public class WalletLockStatus : ProcessBase<WalletLockStatusResponse>
    {
        public WalletLockStatus(WalletConfig settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "walletlockstatus";

        public override string CmdDesc => "Retrieves if wallet is Encrypted and Locked.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<WalletLockStatus> Run()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "walletlockstatus";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            await StartProcess(true, startInfo);

            // Create the Rpc.Result
            if (Rpc.ExitCode != 0)
            {
                await HandleResult(null, startInfo);
                return this;
            }

            var res = JsonConvert.DeserializeObject<WalletLockStatusResponse>(Rpc.OutputMsg);
            Wallet.isLocked = res.isLocked;
            Wallet.isEncrypted = res.isEncrypted;
            await HandleResult(res, startInfo, true);

            return this;
        }
    }
}

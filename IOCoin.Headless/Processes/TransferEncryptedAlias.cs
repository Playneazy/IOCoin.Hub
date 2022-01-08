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
    public class TransferEncryptedAlias : ProcessBase<string>
    {
        public TransferEncryptedAlias(WalletConfig settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "transferencryptedalias";

        public override string CmdDesc => "Transfer an encrypted alias over a channel.";
        public override int TimeoutSec { get; set; } = 30;
        public override double TxFee { get; set; } = 0.02;


        public async Task<TransferEncryptedAlias> Run(string aliasToTransfer, string toAlias, string toAddress)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "transferEncryptedAlias " + aliasToTransfer + " " + toAlias + " " + toAddress;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            await StartProcess(true, startInfo);

            // Create the Rpc.Result
            if (Rpc.ExitCode != 0)
            {
                await HandleResult(String.Empty, startInfo);
                return this;
            }


            await HandleResult(Rpc.OutputMsg, startInfo);
            return this;
            
        }
    }
}

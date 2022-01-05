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
    public class TransferAlias : ProcessBase<string>
    {
        public TransferAlias(Settings settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "transferalias";

        public override string CmdDesc => "Transfer a given alias to the target address or alias.";
        public override int TimeoutSec { get; set; } = 30;
        public override double TxFee { get; set; } = 0.01;


        public async Task<TransferAlias> Run(string alias, string toAddress)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "transferAlias " + alias + " " + toAddress;
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

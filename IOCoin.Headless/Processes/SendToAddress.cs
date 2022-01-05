using IOCoin.Headless.Processes.Interfaces;
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
    public class SendToAddress : ProcessBase<string>
    {
        public SendToAddress(Settings settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "sendtoaddress";

        public override string CmdDesc => "Send an amount of IOC to an address.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<SendToAddress> Run(string toAddress, double value)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "sendtoaddress " + toAddress + " " + value;
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

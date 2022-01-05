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
    public class SendToShade : ProcessBase<ShadeSendResponse>
    {
        public SendToShade(Settings settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "shadesendamount";

        public override string CmdDesc => "Send an amount of IOC to a shade address.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<SendToShade> Run(string toShadeAddress, double value)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "shadesend " + toShadeAddress + " " + value;
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

            var res = JsonConvert.DeserializeObject<ShadeSendResponse>(Rpc.OutputMsg);

            await HandleResult(res, startInfo);

            return this;
            
        }
    }
}

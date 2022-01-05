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
    public class Stop : ProcessBase<bool>
    {
        public Stop(Settings settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "stop";

        public override string CmdDesc => "Stops and closes the daemon.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<Stop> Run()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "stop";
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


            await HandleResult(true, startInfo);
            return this;
            

            
        }
    }
}

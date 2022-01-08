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
    public class CustomCommand : ProcessBase<bool>
    {
        public CustomCommand(WalletConfig settings, IWallet wallet) : base(settings, wallet)
        {

        }

        public override string CmdName => "'Custom Command'";

        public override string CmdDesc => "Attempts to execute a custom command against the daemon.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<CustomCommand> Run(string command)
        {

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + command;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            await StartProcess(true, startInfo);

            // Create the Rpc.Result
            if (Rpc.ExitCode != 0)
            {
                await HandleResult(false, startInfo);
            }

            await HandleResult(true, startInfo);
            return this;
        }
    }
}

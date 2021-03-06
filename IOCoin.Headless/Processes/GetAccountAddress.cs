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
    public class GetAccountAddress : ProcessBase<string>
    {
        public GetAccountAddress(WalletConfig settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "getaccountaddress";

        public override string CmdDesc => "Retrieves account associated with address or leave blank to get primary account.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<GetAccountAddress> Run(string address = "")
        {

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "getaccountaddress \"" + address + "\"";
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

            await HandleResult(Rpc.OutputMsg, startInfo);

            return this;

            
        }
    }
}

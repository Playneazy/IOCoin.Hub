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
    public class CreateShade : ProcessBase<ShadeResponse>
    {
        public CreateShade(WalletConfig settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "createshade";

        public override string CmdDesc => "Creates a shade address for a receiver to be anonymous.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<CreateShade> Run()
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

            var res = JsonConvert.DeserializeObject<IEnumerable<string>>(Rpc.OutputMsg);

            var shadeObj = new ShadeResponse();
            if (res.Any())
            {
                shadeObj.absKey = res.ElementAt(0);
                shadeObj.ordKey = res.ElementAt(1);
                shadeObj.Shade = res.ElementAt(2);
            } else
            {
                await HandleResult(null, startInfo);
            }

            await HandleResult(shadeObj, startInfo);

            return this;
        }
    }
}

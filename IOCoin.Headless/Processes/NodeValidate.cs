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
    public class NodeValidate : ProcessBase<List<AliasResponse>>
    {
        public NodeValidate(WalletConfig settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "nodevalidate";

        public override string CmdDesc => "Retrieves array of aliases and any history of each alias.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<NodeValidate> Run()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "nodeValidate";
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

            var res = JsonConvert.DeserializeObject<List<AliasResponse>>(Rpc.OutputMsg);
            await HandleResult(res, startInfo);

            return this;
        }
    }
}

using IOCoin.Headless.Helpers;
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
    public class GetBlockByNumber : ProcessBase<GetBlockResponse>
    {
        public GetBlockByNumber(WalletConfig settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "getblockbynumber";

        public override string CmdDesc => "Gets a block by the a height or number.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<GetBlockByNumber> Run(long number)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "getblockbynumber " + number;
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

            var res = JsonConvert.DeserializeObject<GetBlockResponse>(Rpc.OutputMsg);
            Wallet.BlockHeight = res.Height;
            await HandleResult(res, startInfo, true);

            return this;
        }
    }
}

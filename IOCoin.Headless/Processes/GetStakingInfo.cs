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
    public class GetStakingInfo : ProcessBase<GetStakingInfoResponse>
    {
        public GetStakingInfo(WalletConfig settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "getstakinginfo";

        public override string CmdDesc => "Returns the staking info.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<GetStakingInfo> Run()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "getstakinginfo";
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

            var res = JsonConvert.DeserializeObject<GetStakingInfoResponse>(Rpc.OutputMsg);
            Wallet.isStaking = res.Enabled;
            await HandleResult(res, startInfo, true);

            return this;
        }
    }
}

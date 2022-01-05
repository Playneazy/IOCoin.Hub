using IOCoin.Headless.Helpers;
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
    public class GetBlockCount : ProcessBase<int>
    {
        public GetBlockCount(Settings settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "getblockcount";

        public override string CmdDesc => "Retrieves latest block number from daemon.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<GetBlockCount> Run()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "getblockcount";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            await StartProcess(true, startInfo);

            // Create the Rpc.Result
            if (Rpc.ExitCode != 0)
            {
                await HandleResult(0, startInfo);
                return this;
            }

            try
            {
                Wallet.BlockHeight = Int32.Parse(Rpc.OutputMsg);
                await HandleResult(Int32.Parse(Rpc.OutputMsg), startInfo, true);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return this;
        }
    }
}

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
    public class GetNumBlocksOfPeers : ProcessBase<int>
    {
        public GetNumBlocksOfPeers(Settings settings, IWallet wallet) : base(settings, wallet)
        {

        }

        public override string CmdName => "getnumblocksofpeers";

        public override string CmdDesc => "Retrieves latest block number from Peers.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<GetNumBlocksOfPeers> Run()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "getnumblocksofpeers";
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
                Wallet.PeerBlockCount = Int32.Parse(Rpc.OutputMsg);
                await HandleResult(Int32.Parse(Rpc.OutputMsg), startInfo, true);
            } catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return this;
        }
    }
}

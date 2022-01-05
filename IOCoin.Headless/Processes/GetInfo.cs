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
    public class GetInfo : ProcessBase<GetInfoResponse>
    {
        public GetInfo(Settings settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "getinfo";

        public override string CmdDesc => "Retrieves critical Blockchain info.";
        public override int TimeoutSec { get; set; } = 30;

        public async Task<GetInfo> Run()
        {
            

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + " getinfo";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            await StartProcess(true, startInfo);

            // Create the Rpc.Result
            if (!string.IsNullOrEmpty(Rpc.ErrorMsg))
            {
                if (Rpc.ErrorMsg.Contains("incorrect rpcuser or rpcpassword"))
                {
                    // try to hook into existing process
                    Process proc = Process.GetProcesses()
                        .Where(p => p.ProcessName == "iocoind").FirstOrDefault();

                    if (proc != null)
                    {
                        Log.Debug($"Attempting to close any running process.");

                        proc.Kill();
                    }
                }
            }
            

            if (Rpc.ExitCode != 0 || !string.IsNullOrEmpty(Rpc.ErrorMsg))
            {
                Wallet.isOnline = false;
                await HandleResult(null, startInfo);
                return this;
            }
           
            var res = JsonConvert.DeserializeObject<GetInfoResponse>(Rpc.OutputMsg);
            Wallet.isOnline = true;
            Wallet.Balance = res.Balance;
            Wallet.BlockHeight = res.Blocks;
            Wallet.ProtocolVersion = res.ProtocolVersion;
            Wallet.Version = res.Version;
            Wallet.WalletVersion = res.WalletVersion;
            Wallet.Pending = res.Pending;
            Wallet.NewMint = res.NewMint;
            Wallet.Stake = res.Stake;
            Wallet.IP = res.IP;
            Wallet.KeypoolOldest = res.KeyPoolOldest;
            Wallet.KeypoolSize = res.KeyPoolSize;
            await HandleResult(res, startInfo, true);

            return this;
            
        }
    }
}

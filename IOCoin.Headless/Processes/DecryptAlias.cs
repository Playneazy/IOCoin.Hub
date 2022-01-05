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
    public class DecryptAlias : ProcessBase<string>
    {
        public DecryptAlias(Settings settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "decryptalias";

        public override string CmdDesc => "Decrypt an alias with associated key address. You can get the associated address by using aliasList.";
        public override int TimeoutSec { get; set; } = 30;
        public override double TxFee { get; set; } = 0.01;


        public async Task<DecryptAlias> Run(string alias, string address)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "decryptAlias " + alias + " " + address;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            await StartProcess(true, startInfo);

            // Create the Rpc.Result
            if (Rpc.ExitCode != 0)
            {
                await HandleResult(String.Empty, startInfo);
                return this;
            }


            await HandleResult(Rpc.OutputMsg, startInfo);
            return this;
            
        }
    }
}
